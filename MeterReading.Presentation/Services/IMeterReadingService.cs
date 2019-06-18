using MeterReading.Core.Models.MeterReading;
using MeterReading.Presentation.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MeterReading.Presentation.Services
{
    public interface IMeterReadingService : IBaseService
    {
        Task<IList<MeterReadingEntry>> GetEntriesAsync();
        Task SaveEntryAsync(IMeterReadingEntryView model);
        Task DeleteEntryAsync(IMeterReadingEntryView model);
        Task PostEntriesAsync(string deviceId, string readingUri, string imageUri, CancellationToken token);
    }
}
