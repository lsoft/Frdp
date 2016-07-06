using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Frdp.Wpf
{
    public class BoolToColorConverter : IValueConverter
    {
        public Brush FalseBrush
        {
            get;
            set;
        }

        public Brush TrueBrush
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return
                (bool)value ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}