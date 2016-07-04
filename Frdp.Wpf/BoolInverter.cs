using System;
using System.Windows.Data;

namespace Frdp.Wpf
{
    public class BoolInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return
                !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return
                !(bool)value;
        }

    }
}