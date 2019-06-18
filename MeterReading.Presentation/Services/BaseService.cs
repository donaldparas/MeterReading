using System.Threading;

namespace MeterReading.Presentation.Services
{
    public class BaseService : IBaseService
    {
        public CancellationTokenSource GetCancellationTokenSource(int? timeoutMilliseconds)
        {
            var cts = new CancellationTokenSource();

            if (timeoutMilliseconds != null)
            {
                cts.CancelAfter((int)timeoutMilliseconds);
            }

            return cts;
        }
    }
}
