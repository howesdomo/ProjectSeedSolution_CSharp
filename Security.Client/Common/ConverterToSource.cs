using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Security.Client.Common
{
    //颜色转换器
    public class ColorToSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool e = true;
            bool.TryParse(value.ToString(),out e);
            if (e)
            {
                return "Black";
            }
            else
            {
                return "Red";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
