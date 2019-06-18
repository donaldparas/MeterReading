using MeterReading.Core.Models.MeterReading;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MeterReading.Core.Services.MeterReadingApi
{
    public interface IMeterReadingApiService
    {
        Task PostMeterReadingAsync(string uri, string deviceId, MeterReadingEntry model, CancellationToken token);
        Task PostMeterImageAsync(string uri, Stream image, string filename, CancellationToken token);
    }
}