using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Sense.Converters {
    public class ImageConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (String.IsNullOrEmpty(value as String) || value.ToString() == "default") {
                return null;
            }
            return
                new BitmapImage(new Uri("pack://application:,,,/Sense;component/Assets/" + value + ".png",
                    UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}