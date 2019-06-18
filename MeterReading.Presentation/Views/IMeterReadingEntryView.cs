using System;

namespace MeterReading.Presentation.Views
{
    public interface IMeterReadingEntryView
    {
        string DeviceId { get; set; }
        string ImagePath { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        string MeterId { get; set; }
        double Reading { get; set; }
        DateTime Timestamp { get; set; }
    }
}
