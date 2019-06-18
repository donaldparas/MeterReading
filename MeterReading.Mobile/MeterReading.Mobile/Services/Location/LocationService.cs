using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MeterReading.Mobile.Services.Location
{
    public class LocationService : ILocationService
    {
        public static readonly ILocationService _instance = new LocationService();

        static LocationService() { }

        private LocationService() { }

        public static ILocationService Instance
        {
            get
            {
                return _instance;
            }
        }

        public async Task<Xamarin.Essentials.Location> GetLocationAsync(GeolocationAccuracy accuracy, TimeSpan timeout)
        {
            var request = new GeolocationRequest(accuracy, timeout);
            var location = await Geolocation.GetLocationAsync(request);

            return location;
        }
    }
}
