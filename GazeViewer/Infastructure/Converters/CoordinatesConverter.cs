using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GazeViewer.Infastructure.Converters
{
    public class CoordinatesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            double pos = 0; Double.TryParse(values[0].ToString(), out pos);
            double k = 0; Double.TryParse(values[1].ToString(), out k);

         //  Debug.WriteLine($"Входные {pos}");
            return pos * (k / 2);
           


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
