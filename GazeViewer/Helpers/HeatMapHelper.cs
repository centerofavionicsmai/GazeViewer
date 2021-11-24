using GazeViewer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GazeViewer.Helpers
{
    class HeatMapHelper
    {
        public List<Ellipse>  GetHeatMap (List<GazePoint> gazePoints,float firstTreshHold = 0.45f, float secondThreshold = 0.2f, float thirdThreshHold = 0.1f)
        {

            List <Ellipse> ellipses= new List<Ellipse>();

            Color blueColor = Color.FromArgb(35, 0, 70, 255);
            Color orangeColor = Color.FromArgb(45, 252, 239, 48);
            Color redColor = Color.FromArgb(45, 227, 5, 27);

            for (int i = 15;i<gazePoints.Count-15; i++)
            {
                if (Math.Abs((gazePoints[i].X - gazePoints[i - 1].X)) <firstTreshHold && Math.Abs((gazePoints[i].Y - gazePoints[i].Y)) < firstTreshHold  )
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.RenderTransform = new TranslateTransform(gazePoints[i].X, gazePoints[i].Y);
                    ellipse.Fill = new SolidColorBrush(blueColor);
                    ellipse.MaxHeight = 12;
                    ellipse.MaxWidth = 12;

                    if (Math.Abs((gazePoints[i].X - gazePoints[i-3].X)) < secondThreshold && Math.Abs((gazePoints[i].Y - gazePoints[i - 3].Y)) < secondThreshold)
                    {
                       ellipse.RenderTransform = new TranslateTransform(gazePoints[i].X, gazePoints[i].Y);
                       ellipse.Fill = new SolidColorBrush(orangeColor);
                    }
                    if (Math.Abs((gazePoints[i].X - gazePoints[i - 7].X)) < thirdThreshHold && Math.Abs((gazePoints[i].Y - gazePoints[i - 7].Y)) < thirdThreshHold)
                    {
                        ellipse.RenderTransform = new TranslateTransform(gazePoints[i].X, gazePoints[i].Y);
                        ellipse.Fill = new SolidColorBrush(redColor);

                    }
                    ellipses.Add(ellipse);
                }
            }

            return ellipses;
        }

    }
}
