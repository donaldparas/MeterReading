using MeterReading.Core.Models.MeterReading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReading.Core.Data.MeterReading
{
    public interface IMeterReadingContext
    {
        Task<IList<MeterReadingEntry>> GetEntriesAsync();
        Task<int> SaveEntryAsync(MeterReadingEntry entry);
        Task<int> DeleteEntryAsync(MeterReadingEntry entry);
    }
}
