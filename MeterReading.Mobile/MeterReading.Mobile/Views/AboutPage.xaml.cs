using System.ComponentModel;
using Xamarin.Forms;

namespace MeterReading.Mobile.Views
{
    [DesignTimeVisible(true)]
    public partial class AboutPage : ContentPage
    {
        private readonly IGlobalSettings _settings;

        public AboutPage(IGlobalSettings settings)
        {
            InitializeComponent();

            _settings = settings;

            AppName.Text = _settings.AppName;
            AppVersion.Text = _settings.AppVersion;
            DeviceId.Text = _settings.DeviceId;
        }

        private void OnProceedButtonClicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainPage(_settings);
        }
    }
}