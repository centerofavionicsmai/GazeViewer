using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace GazeViewer.Models
{
    class GazePoint
    {
        public double XPoint { get; set; }
        public double YPoint { get; set; }
        public double TimeStamp { get; set; }
        [Ignore]
        public SolidColorBrush SolidColorBrush { get; set; }
        public GazePoint()
        {

        }

        public GazePoint(double X,double Y,Color color , double TimeStamp = 0)
        {
            this.XPoint = X;
            this.TimeStamp = TimeStamp;
            this.YPoint = Y;
            this.SolidColorBrush = new SolidColorBrush(color);
        }
    }
}
