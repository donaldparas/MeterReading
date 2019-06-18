using Plugin.Media.Abstractions;
using Xamarin.Essentials;

namespace MeterReading.Mobile
{
    public interface IGlobalSettings
    {
        string AppName { get; }
        string AppVersion { get; }
        string AppCenterAppSecret { get; }
        string DefaultDbPath { get; }
        string DefaultImageDirectory { get; }
        string DefaultImageFileNameSuffix { get; }
        PhotoSize DefaultImageSize { get; }
        GeolocationAccuracy DefaultLocationAccuracy { get; }
        int DefaultLocationRequestTimeoutInSeconds { get; }
        string DeviceId { get; }
        PhotoSize ImageSizeSetting { get; set; }
        GeolocationAccuracy LocationAccuracySetting { get; set; }
        int LocationRequestTimeoutInSecondsSetting { get; set; }
        string MeterImageApiUrl { get; }
        string MeterReadingApiUrl { get; }
        int DefaultTimeoutInMilliSeconds { get; }
        int TimeoutInMilliSeconds { get; set; }
    }
}