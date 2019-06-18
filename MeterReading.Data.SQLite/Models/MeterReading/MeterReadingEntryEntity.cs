using SQLite;
using System;

namespace MeterReading.Data.SQLite.Models.MeterReading
{
    public class MeterReadingEntryEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MeterId { get; set; }
        public double Reading { get; set; }
        public string ImagePath { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
