using GazeViewer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;


namespace GazeViewer.Helpers
{
    class HeatMapHelper
    {
        private DeviceGridItem[,] deviceGrid;

        public void InitDeviceGrid(int width= 1980,int height = 1080)
        {
            deviceGrid = new DeviceGridItem[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    deviceGrid[i, j] = new DeviceGridItem(i-width/2,j-height/2);
                }
            }

        }

        public async Task< List<Path>> GenerateHeatMap(List<GazePointold> gazePoints, float eps = 10, int minIntensivity = 2, int maxIntensivity = 10)
        {
            List<Path> paths = new List<Path>();
            //HeatMap Matrix
            foreach (var p in gazePoints)
            {
                for (int i = 0; i < deviceGrid.GetLength(1)/10; i++)
                {
                    for (int j = 0; j < deviceGrid.GetLength(0)/10; j++)
                    {

                        if ((Math.Abs(deviceGrid[i, j].X - (int)p.X) <= eps) && (Math.Abs(deviceGrid[i, j].Y - (int)p.Y) <= eps))
                        {
                            deviceGrid[i, j].intensivity += 1;
                            if (deviceGrid[i, j].intensivity >= minIntensivity && deviceGrid[i, j].intensivity <= maxIntensivity)
                            {
                                Path path = new Path();
                                Point point = new Point((int)deviceGrid[i, j].X, (int)deviceGrid[i, j].Y);

                                EllipseGeometry ellipseGeometry = new EllipseGeometry(point, 25, 25, new TranslateTransform(deviceGrid[i, j].X, deviceGrid[i, j].Y));
                                path.Data = ellipseGeometry;
                                path.Stroke = new SolidColorBrush(Colors.Black);
                                //    Ellipse ellipse = new Ellipse();
                                //    ellipse.RenderTransform = new TranslateTransform(deviceGrid[i, j].X, deviceGrid[i, j].Y);
                                //    ellipse.Width = 25;
                                //    ellipse.Height = 25;
                                //    ellipse.Fill = new SolidColorBrush(GetColorValue(deviceGrid[i,j].intensivity,minIntensivity,maxIntensivity));
                                //    ellipses.Add(ellipse);
                                //}
                                paths.Add(path);
                            }
                        }
                    }
                }
                // Debug.WriteLine(ellipses.Count);
               
            }
            return paths;
        }

        private Color GetColorValue   (double intensivity,int minIntensivity, int maxIntensivity)
        {
            double pct = (intensivity - minIntensivity)/(maxIntensivity-minIntensivity);
            byte r = Convert.ToByte(255 * pct);
            byte g = Convert.ToByte(255 * (1 - pct));
            byte b = 0;
            return Color.FromArgb(15, r, g, b);
        }
        public List<Ellipse> GetHeatMap(List<GazePointold> gazePoints, float firstTreshHold = 0.45f, float secondThreshold = 0.2f, float thirdThreshHold = 0.1f)
        {

            List<Ellipse> ellipses = new List<Ellipse>();

            Color blueColor = Color.FromArgb(35, 0, 70, 255);
            Color orangeColor = Color.FromArgb(45, 252, 239, 48);
            Color redColor = Color.FromArgb(45, 227, 5, 27);

            for (int i = 15; i < gazePoints.Count - 15; i++)
            {
                if (Math.Abs((gazePoints[i].X - gazePoints[i - 1].X)) < firstTreshHold && Math.Abs((gazePoints[i].Y - gazePoints[i].Y)) < firstTreshHold)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.RenderTransform = new TranslateTransform(gazePoints[i].X, gazePoints[i].Y);
                    ellipse.Fill = new SolidColorBrush(blueColor);
                    ellipse.MaxHeight = 12;
                    ellipse.MaxWidth = 12;

                    if (Math.Abs((gazePoints[i].X - gazePoints[i - 3].X)) < secondThreshold && Math.Abs((gazePoints[i].Y - gazePoints[i - 3].Y)) < secondThreshold)
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
        public class DeviceGridItem
        {
            public double X = 0;
            public double Y = 0;
            public double intensivity = 0;

            public DeviceGridItem(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
