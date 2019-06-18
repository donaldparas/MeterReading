using AutoMapper;
using MeterReading.Core.Data.MeterReading;
using MeterReading.Core.Models.MeterReading;
using MeterReading.Data.SQLite.Models.MeterReading;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReading.Data.SQLite
{
    public class MeterReadingContext : IMeterReadingContext
    {
        readonly SQLiteAsyncConnection _database;
        readonly IMapper _mapper;

        public MeterReadingContext(string dbPath, IMapper mapper)
        {
            _mapper = mapper;
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MeterReadingEntryEntity>().Wait();
        }

        public async Task<IList<MeterReadingEntry>> GetEntriesAsync()
        {
            var records = await _database.Table<MeterReadingEntryEntity>().ToListAsync();

            return _mapper.Map<IList<MeterReadingEntryEntity>, IList<MeterReadingEntry>>(records);
        }

        public async Task<MeterReadingEntry> GetEntryAsync(int id)
        {
            var record = await _database.Table<MeterReadingEntryEntity>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();

            return _mapper.Map<MeterReadingEntryEntity, MeterReadingEntry>(record);
        }

        public async Task<int> SaveEntryAsync(MeterReadingEntry entry)
        {
            var record = _mapper.Map<MeterReadingEntry, MeterReadingEntryEntity>(entry);

            if (entry.ID != 0)
            {
                return await _database.UpdateAsync(record);
            }
            else
            {
                var exists = await _database.Table<MeterReadingEntryEntity>()
                    .Where(i => i.MeterId == entry.MeterId)
                    .FirstOrDefaultAsync();

                if (exists != null)
                {
                    record.ID = exists.ID;
                    return await _database.UpdateAsync(record);
                }

                return await _database.InsertAsync(record);
            }
        }

        public async Task<int> DeleteEntryAsync(MeterReadingEntry entry)
        {
            var record = _mapper.Map<MeterReadingEntry, MeterReadingEntryEntity>(entry);

            return await _database.DeleteAsync(record);
        }
    }
}
