using MeterReading.Core.Data.MeterReading;
using MeterReading.Core.Services.MeterReadingApi;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MeterReading.Core.Models.MeterReading
{
    public class MeterReadingList
    {
        readonly IMeterReadingContext _context;
        readonly IMeterReadingApiService _apiService;

        private MeterReadingList() { }

        public MeterReadingList(IMeterReadingContext context, IMeterReadingApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task<IList<MeterReadingEntry>> GetEntriesAsync()
        {
            return await _context.GetEntriesAsync();
        }

        public async Task<int> SaveEntryAsync(MeterReadingEntry entry)
        {
            return await _context.SaveEntryAsync(entry);
        }

        public async Task<int> DeleteEntryAsync(MeterReadingEntry entry)
        {
            return await _context.DeleteEntryAsync(entry);
        }

        public async Task PostEntriesAsync(string deviceId, string readingUri, string imageUri, CancellationToken token)
        {
            var entries = await _context.GetEntriesAsync();

            foreach (var entry in entries)
            {
                await _apiService.PostMeterReadingAsync(readingUri, deviceId, entry, token);
                await _apiService.PostMeterImageAsync(imageUri, new FileStream(entry.ImagePath, FileMode.Open), entry.Image, token);
                await this.DeleteEntryAsync(entry);
            }
        }
    }
}
