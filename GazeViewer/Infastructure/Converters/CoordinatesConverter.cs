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
          
            double pos = (double)values[0];
            double k =  (double)values[1];

            Debug.WriteLine($"Входные {pos} {k} Выходные {pos * (k / 2)}");
            return pos * (k / 2);
           


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
