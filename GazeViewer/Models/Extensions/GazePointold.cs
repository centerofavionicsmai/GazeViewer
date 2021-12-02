using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeViewer.Extensions
{
  public  class GazePointold
    {
        public double X { get; set; }
        public double Y { get; set; }
        //public DateTime DateTime { get; set; }
        public double timeSpan { get; set; }
        public GazePointold()
        {

        }

        public GazePointold(double x, double y, double _timeSpan)
        {
            X = x;
            Y= y;
            timeSpan = _timeSpan;
            //    DateTime = DateTime.UtcNow; //To do, ov
        }
    }
}
