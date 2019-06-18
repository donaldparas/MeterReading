using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeterReading.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        private readonly IGlobalSettings _settings;

        public MainPage(IGlobalSettings settings)
        {
            InitializeComponent();

            _settings = settings;

            Children.Add(new NavigationPage(new MeterReadingListPage()));
            Children.Add(new AboutPage(_settings));

            Children[0].Title = "Browse";
            Children[0].Icon = "tab_feed.png";

            Children[1].Title = "About";
            Children[1].Icon = "tab_about.png";
        }
    }
}