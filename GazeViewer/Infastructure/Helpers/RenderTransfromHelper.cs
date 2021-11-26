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

        public List<GazePointold> TransformToWindowCoordinates(List<GazePointold> points, double Width, double Height)
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

     
        public (double, double) TransformToWindow (  (double _x,double _y) normValues , double Width, double Height)
        {
            double x = normValues._x * (Width/2);
            double y = normValues._y * (Height/2);

            return (x,y);
        }




        public List<GazePointold> TransformToNormalizeCoordinate(List<GazePointold> points,double Width,double Height)
        {
            foreach (var p in points)
            {
                double normX = p.X / (Width / 2);
                double normY = p.Y / (Height / 2);
                //debug
                {
                    //Ellipse ellipse = new Ellipse();
                    //ellipse.MaxHeight = 5;
                    //ellipse.MaxWidth = 5;
                    //ellipse.Fill = new SolidColorBrush(Colors.Black);
                    //ellipse.RenderTransform = new TranslateTransform(x, y);
                    //mainGrid.Children.Add(ellipse);

                }
                p.X = normX;
                p.Y = normY;
            }
            return points;

        }
    }
}
