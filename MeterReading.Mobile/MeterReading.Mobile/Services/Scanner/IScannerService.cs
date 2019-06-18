using MeterReading.Mobile.ViewModels.MeterReading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeterReading.Mobile.Services.Scanner
{
    public interface IScannerService
    {
        Task ScanAsync(INavigation navigation, MeterReadingEntryViewModel model);
    }
}
