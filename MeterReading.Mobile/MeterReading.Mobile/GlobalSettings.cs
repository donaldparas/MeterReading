using Microsoft.AppCenter;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Essentials;

namespace MeterReading.Mobile
{
    public class GlobalSettings : IGlobalSettings
    {
        // Azure App Center
        public string AppCenterAppSecret => "0f745df8-4981-4962-93e0-b43311587a6d";
        // App Info
        public string AppName => AppInfo.Name;
        public string AppVersion => $"{ AppInfo.VersionString } ({ AppInfo.BuildString })";

        // Device Id
        public string DeviceId => AppCenter.GetInstallIdAsync().Result.ToString();

        // Meter API URL
        public string MeterReadingApiUrl => _meterApiBaseUrl + "/Meter/Reading";
        public string MeterImageApiUrl => _meterApiBaseUrl + "/Meter/Image";
        public int DefaultTimeoutInMilliSeconds => 10000;
        public int TimeoutInMilliSeconds { get; set; }

        // DB path
        public string DefaultDbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MeterReading.db3");

        // Capture image settings
        public string DefaultImageDirectory => "MeterUtility";
        public string DefaultImageFileNameSuffix => "_" + DeviceId.ToString().Substring(0, DeviceId.IndexOf("-"))  + ".jpg";
        public PhotoSize DefaultImageSize => PhotoSize.Small;
        public PhotoSize ImageSizeSetting { get; set; }

        // Location request settings
        public GeolocationAccuracy DefaultLocationAccuracy => GeolocationAccuracy.Medium;
        public int DefaultLocationRequestTimeoutInSeconds => 10;
        public GeolocationAccuracy LocationAccuracySetting { get; set; }
        public int LocationRequestTimeoutInSecondsSetting { get; set; }

        public static IGlobalSettings Instance { get; } = new GlobalSettings();

        static GlobalSettings() { }

        private GlobalSettings()
        {
            ImageSizeSetting = DefaultImageSize;
            LocationAccuracySetting = DefaultLocationAccuracy;
            LocationRequestTimeoutInSecondsSetting = DefaultLocationRequestTimeoutInSeconds;
            TimeoutInMilliSeconds = DefaultTimeoutInMilliSeconds;
        }

        // Environment-specific settings
#if DEBUG
        private readonly string _meterApiBaseUrl = "http://172.18.9.23:5000/MeterReadingAPI/v1";
#endif
#if RELEASE
        private readonly string _meterApiBaseUrl = "http://172.18.5.58:5000/MeterReadingAPI/v1";
#endif
    }
}
