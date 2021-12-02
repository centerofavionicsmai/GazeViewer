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
        public double XGaze { get; set; }
        public double YGaze{ get; set; } 
       

        public GazePoint()
        {

        }

        public GazePoint(double X,double Y)
        {
            this.XGaze = X;
            this.YGaze = Y;
        }
    }
}
