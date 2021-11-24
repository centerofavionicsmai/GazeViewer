using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeViewer.Extensions
{
  public  class GazePoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        //public DateTime DateTime { get; set; }
        public double timeSpan { get; set; }
        public GazePoint()
        {

        }

        public GazePoint(double x, double y, double _timeSpan)
        {
            X = x;
            Y = y;
            timeSpan = _timeSpan;
            //    DateTime = DateTime.UtcNow; //To do, ov
        }
    }
}
