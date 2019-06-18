using MeterReading.Mobile.ViewModels.MeterReading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace MeterReading.Mobile.Services.Scanner
{
    public class ScannerService : IScannerService
    {
        public static readonly IScannerService _instance = new ScannerService();

        static ScannerService() { }

        private ScannerService() { }

        public static IScannerService Instance
        {
            get
            {
                return _instance;
            }
        }

        public async Task ScanAsync(INavigation navigation, MeterReadingEntryViewModel model)
        {
            var _navigation = navigation;

            var scanPage = new ZXingScannerPage();

            MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions()
            {
                AutoRotate = true
            };

            var overlay = new ZXingDefaultOverlay
            {
                TopText = "Align the barcode within the frame",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = true

            };
            overlay.BindingContext = overlay;

            overlay.FlashButtonClicked += delegate
            {
                scanPage.ToggleTorch();
            };

            scanPage = new ZXingScannerPage(options, overlay);

            scanPage.OnScanResult += (result) =>
            {
                App.Analytics.TrackEvent("After barcode scanning");

                scanPage.IsScanning = false;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _navigation.PopAsync();
                    model.MeterId = result.Text;
                });
            };

            await _navigation.PushAsync(scanPage);
        }
    }
}
