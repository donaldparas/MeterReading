using MeterReading.Mobile.ViewModels.MeterReading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeterReading.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeterReadingListPage : ContentPage
    {
        protected MeterReadingListViewModel Model
        {
            get { return (MeterReadingListViewModel)BindingContext; }
        }

        public MeterReadingListPage()
        {
            InitializeComponent();
            var model = new MeterReadingListViewModel(navigation: Navigation);
            BindingContext = model;
        }

        public MeterReadingListPage(MeterReadingListViewModel model)
        {
            InitializeComponent();
            this.BindingContext = model;
        }
    }
}