using System;
using System.IO;

namespace MeterReading.Core.Models.MeterReading
{
    public class MeterReadingEntry
    {
        public int ID { get; set; }
        public string MeterId { get; set; }
        public double Reading { get; set; }
        public string ImagePath { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Timestamp { get; set; }

        public string Image
        {
            get
            {
                return Path.GetFileName(ImagePath);
            }
        }
    }
}
