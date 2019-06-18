using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace MeterReading.Mobile.Converters
{
    public class StreamToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return ImageSource.FromStream(() => (Stream)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
