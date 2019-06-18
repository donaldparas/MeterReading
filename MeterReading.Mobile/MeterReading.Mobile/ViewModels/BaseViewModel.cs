using Acr.UserDialogs;
using MeterReading.Mobile.Services.Analytics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MeterReading.Mobile.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private IAnalyticsService _analyticsService;
        private IUserDialogs _dialogService;

        protected BaseViewModel() : this(App.Analytics, App.Dialogs) { }

        protected BaseViewModel(IAnalyticsService analyticsService, IUserDialogs dialogService)
        {
            _analyticsService = analyticsService ?? App.Analytics;
            _dialogService = dialogService ?? App.Dialogs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void TrackEvent(string name)
        {
            _analyticsService.TrackEvent(name);
        }

        protected virtual async Task HandleException(string action, string errorCode, object properties, Exception err)
        {
            _analyticsService.TrackError(err, new Dictionary<string, string> {
                    { "Event", action },
                    { "Entry", JsonConvert.SerializeObject(properties) }
                });
            Debug.WriteLine($"Error={err.ToString()}");
            await _dialogService.AlertAsync($"Unexpected error occurred.  Err Code={errorCode}", "Error", "OK");
        }
    }
}
