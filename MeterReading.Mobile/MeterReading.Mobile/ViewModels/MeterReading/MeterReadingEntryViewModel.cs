using Acr.UserDialogs;
using MeterReading.Mobile.Services.Analytics;
using MeterReading.Mobile.Services.Camera;
using MeterReading.Mobile.Services.Location;
using MeterReading.Mobile.Services.Scanner;
using MeterReading.Presentation.Services;
using MeterReading.Presentation.Views;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeterReading.Mobile.ViewModels.MeterReading
{
    public class MeterReadingEntryViewModel : BaseViewModel, IMeterReadingEntryView
    {
        private string _deviceId;
        private string _meterId;
        private double _reading;
        private string _readingStr;
        private double _longitude;
        private double _latitude;
        private string _imagePath;
        private ImageSource _imageSource;
        private DateTime _timestamp;

        private INavigation _navigation;
        private readonly IGlobalSettings _settings;
        private readonly IMeterReadingService _service;
        private readonly IUserDialogs _dialogService;
        private readonly ICameraService _cameraService;
        private readonly ILocationService _locationService;
        private readonly IScannerService _scannerService;

        public MeterReadingEntryViewModel() : base(App.Analytics, App.Dialogs)
        {
            _navigation = App.Current.MainPage.Navigation;
            _settings = App.Settings;
            _service = App.MeterReadingService;
            _dialogService = App.Dialogs;
            _cameraService = App.Camera;
            _locationService = App.Location;
            _scannerService = App.Scanner;
        }

        public MeterReadingEntryViewModel(INavigation navigation
            , IGlobalSettings settings
            , IMeterReadingService service
            , IAnalyticsService analyticsService
            , IUserDialogs dialogService
            , ICameraService cameraService
            , ILocationService locationService
            , IScannerService scannerService) : base(analyticsService, dialogService)
        {
            _settings = settings;
            _service = service;
            _dialogService = dialogService;
            _navigation = navigation;
            _cameraService = cameraService;
            _locationService = locationService;
            _scannerService = scannerService;
        }

        public void Initialize(INavigation navigation)
        {
            _navigation = navigation;
        }

        public ICommand AppearingCommand => new Command(OnAppearing);
        public ICommand SaveCommand => new Command(async () => await SaveAsync());
        public ICommand DeleteCommand => new Command(async () => await DeleteAsync());
        public ICommand TakePhotoCommand => new Command(async () => await TakePhotoAsync());
        public ICommand ScanCommand => new Command(async () => await ScanAsync());

        public int Id { get; set; }

        public string DeviceId
        {
            get => _deviceId;
            set
            {
                if (_deviceId != value)
                {
                    _deviceId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MeterId
        {
            get => _meterId;
            set
            {
                if (_meterId != value)
                {
                    _meterId = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Reading
        {
            get => _reading;
            set
            {
                if (_reading != value)
                {
                    _reading = value;
                    OnPropertyChanged();

                    ReadingStr = _reading.ToString();

                    if (_reading == 0)
                    {
                        ReadingStr = null;
                    }
                }
            }
        }

        public string ReadingStr
        {
            get => _readingStr;
            set
            {
                if (_readingStr != value)
                {
                    _readingStr = value;
                    OnPropertyChanged();

                    Reading = double.Parse(_readingStr);
                }
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Latitude
        {
            get => _latitude;
            set
            {
                if (_latitude != value)
                {
                    _latitude = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                    ImageSource = ImageSource.FromStream(() => { return new FileStream(_imagePath, FileMode.Open); });
                }
            }
        }

        [JsonIgnore]
        public ImageSource ImageSource
        {
            get => _imageSource;
            private set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnAppearing()
        {
            TrackEvent($"MeterReadingEntry {nameof(OnAppearing)}");
        }

        private async Task SaveAsync()
        {
            TrackEvent($"MeterReadingEntry {nameof(SaveAsync)}");

            if (MeterId == null || ImagePath == null || MeterId.Trim().Length == 0)
            {

                await _dialogService.AlertAsync("Meter ID and photo are required.", "Error", "OK");
                return;
            }

            Timestamp = DateTime.Now;

#if DEBUG
            if (!await _dialogService.ConfirmAsync($"Device ID: {DeviceId}\n" +
                              $"Meter ID: {MeterId}\n" +
                              $"Reading: {Reading}\n" +
                              $"Location: {Longitude}, {Latitude}\n" +
                              $"ImagePath: {ImagePath}\n" +
                              $"Timestamp: {Timestamp}"
                              , "Confirm entry details"))
            {
                return;
            }
#endif

            try
            {
                using (var loading = _dialogService.Loading("Saving"))
                {
                    await _service.SaveEntryAsync(this);
                }
            }
            catch (Exception err)
            {
                await HandleException(nameof(SaveAsync), @"201", this, err);
            }

            await _navigation.PopAsync();
        }

        private async Task DeleteAsync()
        {
            TrackEvent($"MeterReadingEntry {nameof(DeleteAsync)}");

            try
            {
                if (!await _dialogService.ConfirmAsync("Are you sure?", "Delete"))
                {
                    return;
                }

                using (var loading = _dialogService.Loading("Deleting"))
                {
                    await _service.DeleteEntryAsync(this);
                }
            }
            catch (Exception err)
            {
                await HandleException(nameof(DeleteAsync), @"202", this, err);
            }

            await _navigation.PopAsync();
        }

        private async Task TakePhotoAsync()
        {
            TrackEvent($"MeterReadingEntry {nameof(TakePhotoAsync)}");

            try
            {
                var imagePath = await _cameraService.TakePhotoAsync(_settings.DefaultImageSize, _settings.DefaultImageDirectory, _settings.DefaultImageFileNameSuffix);

                if (imagePath != null)
                {
                    ImagePath = imagePath;

                    var location = await _locationService.GetLocationAsync(_settings.LocationAccuracySetting, TimeSpan.FromSeconds(_settings.LocationRequestTimeoutInSecondsSetting));

                    if (location != null)
                    {
                        Longitude = location.Longitude;
                        Latitude = location.Latitude;
                    }
                }
            }
            catch (Exception err)
            {
                await HandleException(nameof(TakePhotoAsync), @"203", this, err);
            }
        }

        private async Task ScanAsync()
        {
            TrackEvent($"MeterReadingEntry {nameof(ScanAsync)}");

            try
            {
                await _scannerService.ScanAsync(_navigation, this);
            }
            catch (Exception err)
            {
                await HandleException(nameof(ScanAsync), @"204", this, err);
            }
        }
    }
}
