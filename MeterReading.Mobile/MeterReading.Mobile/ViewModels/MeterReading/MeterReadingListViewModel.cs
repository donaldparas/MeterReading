using Acr.UserDialogs;
using AutoMapper;
using MeterReading.Core.Models.MeterReading;
using MeterReading.Mobile.Services.Analytics;
using MeterReading.Mobile.Views;
using MeterReading.Presentation.Services;
using MeterReading.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeterReading.Mobile.ViewModels.MeterReading
{
    public class MeterReadingListViewModel : BaseViewModel, IMeterReadingListView
    {
        private readonly IGlobalSettings _settings;
        private readonly IMapper _mapper;
        private readonly IUserDialogs _dialogService;
        private readonly INavigation _navigation;
        private readonly IMeterReadingService _service;

        private ObservableCollection<MeterReadingEntryViewModel> _entriesCollection;
        private IList<MeterReadingEntry> _entries;
        private MeterReadingEntryViewModel _selectedEntry;

        public MeterReadingListViewModel() : base(App.Analytics, App.Dialogs)
        {
            _settings = App.Settings;
            _mapper = App.MapperConfig.CreateMapper();
            _dialogService = App.Dialogs;
            _navigation = App.Current.MainPage.Navigation;
            _service = App.MeterReadingService;
        }

        public MeterReadingListViewModel(IGlobalSettings settings = null
            , IMapper mapper = null
            , IAnalyticsService analyticsService = null
            , IUserDialogs dialogService = null
            , INavigation navigation = null
            , IMeterReadingService service = null) : base(analyticsService, dialogService)
        {
            _settings = settings ?? App.Settings;
            _mapper = mapper ?? App.MapperConfig.CreateMapper();
            _dialogService = dialogService ?? App.Dialogs;
            _navigation = navigation ?? App.Current.MainPage.Navigation;
            _service = service ?? App.MeterReadingService;
        }

        public ObservableCollection<MeterReadingEntryViewModel> EntriesCollection
        {
            get { return _entriesCollection; }
            set
            {
                if (_entriesCollection != value)
                {
                    _entriesCollection = value;
                    OnPropertyChanged();
                }
            }
        }

        public MeterReadingEntryViewModel SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                if (_selectedEntry != value)
                {
                    _selectedEntry = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AppearingCommand => new Command(async () => await OnAppearingAsync());
        public ICommand AddItemCommand => new Command(async () => await AddItemAsync());
        public ICommand SelectedEntryCommand => new Command<MeterReadingEntryViewModel>(async e => await SelectedEntryAsync(e));
        public ICommand PostEntriesCommand => new Command(async () => await PostEntriesAsync());

        private async Task OnAppearingAsync()
        {
            TrackEvent($"MeterReadingList {nameof(OnAppearingAsync)}");
            SetEntries(await _service.GetEntriesAsync());
        }

        private async Task AddItemAsync()
        {
            TrackEvent($"MeterReadingList {nameof(AddItemAsync)}");
            await _navigation.PushAsync(new MeterReadingEntryPage(new MeterReadingEntryViewModel()));
        }

        private async Task SelectedEntryAsync(MeterReadingEntryViewModel model)
        {
            TrackEvent($"MeterReadingList {nameof(SelectedEntryAsync)}");
            await _navigation.PushAsync(new MeterReadingEntryPage(model));
        }

        private async Task PostEntriesAsync()
        {
            TrackEvent($"MeterReadingList {nameof(PostEntriesAsync)}");

            if (!await _dialogService.ConfirmAsync("Are you sure?", "Post"))
            {
                return;
            }

            try
            {
                using (var loading = _dialogService.Loading("Posting"))
                using (var cts = _service.GetCancellationTokenSource(_settings.TimeoutInMilliSeconds))
                {
                    await _service.PostEntriesAsync(_settings.DeviceId, _settings.MeterReadingApiUrl, _settings.MeterImageApiUrl, cts.Token);
                    SetEntries(await _service.GetEntriesAsync());
                }
            }
            catch (Exception err)
            {
                await HandleException(nameof(PostEntriesAsync), @"101", this, err);
            }
        }

        private void SetEntries(IList<MeterReadingEntry> value)
        {
            if (_entries != value)
            {
                _entries = value;
                var collection = new ObservableCollection<MeterReadingEntryViewModel>(_mapper.Map<IList<MeterReadingEntry>, IList<MeterReadingEntryViewModel>>(_entries));
                EntriesCollection = collection;
            }
        }
    }
}
