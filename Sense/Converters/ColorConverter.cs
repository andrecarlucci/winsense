using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SharpSenses;
using Color = System.Drawing.Color;

namespace Sense.Converters {
    public class ColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var part = value as FlexiblePart;
            if (part != null) {
                if (part.IsOpen) {
                    return new SolidColorBrush(Colors.CadetBlue);
                }
                return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.CadetBlue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}