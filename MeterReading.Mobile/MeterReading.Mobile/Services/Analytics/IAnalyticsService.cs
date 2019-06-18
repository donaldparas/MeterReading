using System;
using System.Collections.Generic;

namespace MeterReading.Mobile.Services.Analytics
{
    public interface IAnalyticsService
    {
        void TrackEvent(string name);
        void TrackError(Exception exception, IDictionary<string, string> properties);
    }
}
