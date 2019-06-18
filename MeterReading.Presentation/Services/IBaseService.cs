using System.Threading;

namespace MeterReading.Presentation.Services
{
    public interface IBaseService
    {
        CancellationTokenSource GetCancellationTokenSource(int? timeoutMilliseconds);
    }
}