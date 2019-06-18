using AutoMapper;
using System;

namespace MeterReading.Services.MeterReadingAPI.Models.MeterReading
{
    public class MeterReadingEntryApiModel
    {
        public string MeterId { get; set; }
        public double Reading { get; set; }
        public string Image { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Timestamp { get; set; }
        [IgnoreMap]
        public string DeviceId { get; set; }
    }
}
