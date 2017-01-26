using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Harris.CelestialADB.Desktop.Converters
{
    public class BooleanToBlurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Boolean)
            {
                if (parameter is string)
                {
                    if ((string)parameter == "True")
                        return (!(bool)value) ? 6 : 0;
                    else
                        return ((bool)value) ? 6 : 0;
                }
                else
                {
                    return ((bool)value) ? 6 : 0;
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
