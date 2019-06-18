using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;

namespace MeterReading.Mobile.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        public static readonly IAnalyticsService _instance = new AnalyticsService();

        static AnalyticsService() { }

        private AnalyticsService() { }

        public static IAnalyticsService Instance
        {
            get
            {
                return _instance;
            }
        }

        public void TrackError(Exception exception, IDictionary<string, string> properties)
        {
            Crashes.TrackError(exception, properties);
        }

        public void TrackEvent(string name)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(name);
        }
    }
}
