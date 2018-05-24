using ServiceGUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ServiceGUI.View
{
    class ConnectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            bool item = ConnectionChecker.IsConnected();

            Color clr;

            //  Add code here to pick a color or generate RGB values for one
            if (item)
                clr = Colors.White;
            else
                clr = Colors.Gray;

            return new SolidColorBrush(clr);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
