using System.Globalization;
using System.Windows.Data;

namespace Innovators_ShareMarket.Models
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null &&
                value is DateTime dateTime)
            {
                return dateTime.ToString("hh:mm tt");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
