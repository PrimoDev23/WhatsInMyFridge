using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhatsInMyFridge.Converter
{
    public class FridgeStateToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return null;
            }

            if((bool)value == true)
            {
                return new SolidColorBrush(Color.Green);
            }
            else
            {
                return new SolidColorBrush(Color.IndianRed);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
