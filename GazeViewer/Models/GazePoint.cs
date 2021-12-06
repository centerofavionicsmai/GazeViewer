using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GazeViewer.Models
{
    class GazePoint
    {
        public double XPoint { get; set; }
        public double YPoint { get; set; }
        public double TimeStamp { get; set; }

        public GazePoint()
        {

        }

        public GazePoint(double X,double Y,double TimeStamp=0)
        {
            this.XPoint = X;
            this.TimeStamp = TimeStamp;
            this.YPoint = Y;
        }
    }
}
