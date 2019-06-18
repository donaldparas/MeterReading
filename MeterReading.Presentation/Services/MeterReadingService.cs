using AutoMapper;
using MeterReading.Core.Models.MeterReading;
using MeterReading.Presentation.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MeterReading.Presentation.Services
{
    public class MeterReadingService : BaseService, IMeterReadingService
    {
        readonly IMapper _mapper;
        readonly MeterReadingList _list;

        private MeterReadingService() { }

        public MeterReadingService(IMapper mapper, MeterReadingList list)
        {
            _mapper = mapper;
            _list = list;
        }

        public async Task<IList<MeterReadingEntry>> GetEntriesAsync()
        {
            return await _list.GetEntriesAsync();
        }

        public async Task DeleteEntryAsync(IMeterReadingEntryView model)
        {
            var entry = _mapper.Map<IMeterReadingEntryView, MeterReadingEntry>(model);
            await _list.DeleteEntryAsync(entry);
        }

        public async Task SaveEntryAsync(IMeterReadingEntryView model)
        {
            var entry = _mapper.Map<IMeterReadingEntryView, MeterReadingEntry>(model);
            await _list.SaveEntryAsync(entry);
        }

        public async Task PostEntriesAsync(string deviceId, string readingUri, string imageUri, CancellationToken token)
        {
            await _list.PostEntriesAsync(deviceId, readingUri, imageUri, token);
        }
    }
}
