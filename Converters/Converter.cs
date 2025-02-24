using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace SoundBrewPOS.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Colors.Maroon : Colors.LightGray; // Adjust colors as needed
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
