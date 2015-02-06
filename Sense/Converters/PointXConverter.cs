using System;
using System.Globalization;
using System.Windows.Data;
using Sense.Util;

namespace Sense.Converters {
    public class PointXConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return CameraToScreenMapper.MapXToScreen((double) value, MainWindow.MyWidth);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}