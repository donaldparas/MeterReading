using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MeterReading.Mobile.Services.Location
{
    public interface ILocationService
    {
        Task<Xamarin.Essentials.Location> GetLocationAsync(GeolocationAccuracy accuracy, TimeSpan timeout);
    }
}