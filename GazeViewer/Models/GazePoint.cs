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
        public double X { get; set; }
        public double Y{ get; set; } 
       

        public GazePoint()
        {

        }

        public GazePoint(double X,double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
