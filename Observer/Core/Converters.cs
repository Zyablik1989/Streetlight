using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Observer.Core.Converters
{
        public class BoolToStreetLightColor : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if ((value is bool) &&
                    ((bool)value))
                {
                    return new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
            else if (value is int)
                return (int)value == 0 ? Visibility.Hidden : Visibility.Visible;
            else if (value is string)
                return string.IsNullOrEmpty((string)value) ? Visibility.Hidden : Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
