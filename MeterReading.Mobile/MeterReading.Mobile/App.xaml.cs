using Acr.UserDialogs;
using AutoMapper;
using MeterReading.Core.Models.MeterReading;
using MeterReading.Core.Services.MeterReadingApi;
using MeterReading.Data.SQLite;
using MeterReading.Data.SQLite.Models.MeterReading;
using MeterReading.Mobile.Services.Analytics;
using MeterReading.Mobile.Services.Camera;
using MeterReading.Mobile.Services.Location;
using MeterReading.Mobile.Services.Scanner;
using MeterReading.Mobile.ViewModels.MeterReading;
using MeterReading.Mobile.Views;
using MeterReading.Presentation.Services;
using MeterReading.Services.MeterReadingAPI;
using MeterReading.Services.MeterReadingAPI.Models.MeterReading;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;

namespace MeterReading.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!DesignMode.IsDesignModeEnabled)
            {
                AppCenter.Start($"android={Settings.AppCenterAppSecret};"
                    , typeof(Analytics)
                    , typeof(Crashes)
                    , typeof(Distribute)
                    , typeof(Push));
                AppCenter.SetUserId(Settings.DeviceId);
            }

            MainPage = new AboutPage(App.Settings);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if (!DesignMode.IsDesignModeEnabled)
            {
                Analytics.TrackEvent("Application started");
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public readonly static IGlobalSettings Settings = GlobalSettings.Instance;

        public readonly static MapperConfiguration MapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MeterReadingEntry, MeterReadingEntryViewModel>().ReverseMap();
            cfg.CreateMap<MeterReadingEntry, MeterReadingEntryEntity>().ReverseMap();
            cfg.CreateMap<MeterReadingEntry, MeterReadingEntryApiModel>().ReverseMap();
        });

        public readonly static IAnalyticsService Analytics = AnalyticsService.Instance;
        public readonly static ICameraService Camera = CameraService.Instance;
        public readonly static ILocationService Location = LocationService.Instance;
        public readonly static IScannerService Scanner = ScannerService.Instance;
        public readonly static IUserDialogs Dialogs = UserDialogs.Instance;

        public readonly static IMeterReadingApiService MeterReadingApi = new MeterReadingApiService(MapperConfig.CreateMapper());

        private readonly static MeterReadingContext context = new MeterReadingContext(Settings.DefaultDbPath, MapperConfig.CreateMapper());

        public readonly static MeterReadingList MeterReadingList = new MeterReadingList(context, MeterReadingApi);

        public readonly static MeterReadingService MeterReadingService = new MeterReadingService(MapperConfig.CreateMapper(), MeterReadingList);
    }
}
