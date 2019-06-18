using MeterReading.Mobile.ViewModels.MeterReading;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeterReading.Mobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomListView : ContentView
    {
        public static readonly BindableProperty EntriesProperty = BindableProperty.Create(nameof(Entries), typeof(IList<MeterReadingEntryViewModel>), typeof(CustomListView));
        public static readonly BindableProperty SelectedEntryProperty = BindableProperty.Create(nameof(SelectedEntry), typeof(MeterReadingEntryViewModel), typeof(CustomListView));
        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(CustomListView));

        public IList<MeterReadingEntryViewModel> Entries
        {
            get => (IList<MeterReadingEntryViewModel>)GetValue(EntriesProperty);
            set => SetValue(EntriesProperty, value);
        }

        public MeterReadingEntryViewModel SelectedEntry
        {
            get => (MeterReadingEntryViewModel)GetValue(SelectedEntryProperty);
            set => SetValue(SelectedEntryProperty, value);
        }

        public ICommand ItemSelectedCommand
        {
            get => (ICommand)GetValue(ItemSelectedCommandProperty);
            set => SetValue(ItemSelectedCommandProperty, value);
        }

        public CustomListView()
        {
            InitializeComponent();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ItemSelectedCommand != null)
            {
                ItemSelectedCommand.Execute(e.SelectedItem);
            }
        }
    }
}