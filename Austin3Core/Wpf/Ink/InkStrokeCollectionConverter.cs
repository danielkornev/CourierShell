using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Ink;

namespace ZU.Shared.Wpf.Ink
{
    public class InkStrokeCollectionConverter : IValueConverter
    {
        public static StrokeCollectionConverter InternalInstance = new StrokeCollectionConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var str = value as string;

            if (String.IsNullOrEmpty(str)) return null;

            try
            {
                return InternalInstance.ConvertFrom(str) as StrokeCollection;
            }
            catch
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;

            var str = value as StrokeCollection;

            if (str == null) return string.Empty;

            try
            {
                return InternalInstance.ConvertToString(str);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
