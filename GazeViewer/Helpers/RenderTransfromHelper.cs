using GazeViewer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GazeViewer.Helpers
{
    class RenderTransfromHelper
    {

        public List<GazePoint> TransformToWindowCoordinates(ref List<GazePoint> points, double Width,double Height )
        {
            foreach (var p in points)
            {

                double x = p.X * (Width / 2);
                double y = p.Y * (Height / 2);
                p.X = x;
                p.Y = y;
            }
            return points;
        }
    }
}
