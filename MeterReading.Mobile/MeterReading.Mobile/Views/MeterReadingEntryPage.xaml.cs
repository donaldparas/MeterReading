using MeterReading.Mobile.ViewModels.MeterReading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeterReading.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeterReadingEntryPage : ContentPage
    {
        protected MeterReadingEntryViewModel Model
        {
            get { return (MeterReadingEntryViewModel)BindingContext; }
        }

        public MeterReadingEntryPage() : this(new MeterReadingEntryViewModel()) { }

        public MeterReadingEntryPage(MeterReadingEntryViewModel model)
        {
            InitializeComponent();
            this.BindingContext = model;
            Model.Initialize(Navigation);
        }
    }
}