using SharedInfo.Messages;
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
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var item = value as MessageRecievedEventArgs;

            Color clr;

            //  Add code here to pick a color or generate RGB values for one
            switch (item.Status)
            {
                case MessageTypeEnum.FAIL:
                    clr = Colors.Red;
                    break;
                case MessageTypeEnum.INFO:
                    clr = Colors.Green;
                    break;
                case MessageTypeEnum.WARNING:
                    clr = Colors.Yellow;
                    break;
                default:
                    clr = Colors.White;
                    break;

            }

            return new SolidColorBrush(clr);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
