using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GazeViewer.Helpers;
using GazeViewer.Extensions;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace GazeViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<GazePoint> GazePoints;
        private CsvFileReader csvFileReader;
        private RenderTransfromHelper renderTransfromHelper;
        private HeatMapHelper heatMapHelper;
        private int GazePointIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            List<GazePoint> GenerateGazePoints = new List<GazePoint>();
            var a = WriteTestGazePoints(100, 150, -100, 100,5);
            GenerateGazePoints.AddRange(a);
            CSVWriter cSVWriter = new CSVWriter();
            cSVWriter.WriteGazePoints(GenerateGazePoints);

            InitHelpers();
        }

        private void InitHelpers()
        {
          csvFileReader  = new CsvFileReader();
          renderTransfromHelper  = new RenderTransfromHelper();
          heatMapHelper = new HeatMapHelper();
        }


        #region Drawing
        public void DrawHeatMap(List<Ellipse> ellipses)
        {
            foreach (var r in ellipses)
            {
                mainGrid.Children.Add(r);
            }
        }

        public void DrawGazePoints()
        {
            foreach (var point in GazePoints)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = GazePoints.IndexOf(point).ToString();
                textBlock.FontSize = 16;
                textBlock.Foreground = new SolidColorBrush(Colors.White);
                textBlock.RenderTransform = new TranslateTransform(point.X + Width / 2 - 5, point.Y - 10 + Height / 2);
                Ellipse ellipse = new Ellipse();
                ellipse.MaxHeight = 28;
                ellipse.MaxWidth = 28;
                ellipse.RenderTransform = new TranslateTransform(point.X, point.Y);
                ellipse.Fill = new SolidColorBrush(Color.FromArgb(65, 255, 255, 255));
                mainGrid.Children.Add(ellipse);
                mainGrid.Children.Add(textBlock);
            }
        }

        public void DrawGazePoint(GazePoint gazePoint,Color color)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = GazePoints.IndexOf(gazePoint).ToString();
            textBlock.FontSize = 16;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.RenderTransform = new TranslateTransform(gazePoint.X + Width / 2 - 5, gazePoint.Y - 10 + Height / 2);
            Ellipse ellipse = new Ellipse();
            ellipse.MaxHeight = 28;
            ellipse.MaxWidth = 28;
            ellipse.RenderTransform = new TranslateTransform(gazePoint.X, gazePoint.Y);
            ellipse.Fill = new SolidColorBrush(color);
            mainGrid.Children.Add(ellipse);
            mainGrid.Children.Add(textBlock);
        }

        public void DrawGazePlot(int index)
        {
            Polyline polyline = new Polyline();
            Point point = new Point(GazePoints[GazePointIndex - 2].X + Width/2, GazePoints[GazePointIndex - 2].Y + Height/2);
            polyline.Points.Add(point);
            point = new Point(GazePoints[GazePointIndex-1].X + Width/2, GazePoints[GazePointIndex-1].Y + Height/2);
            polyline.Points.Add(point);
            polyline.Stroke = new SolidColorBrush(Colors.White);
            polyline.StrokeThickness = 4;
            mainGrid.Children.Add(polyline);
        }

        public void DrawGazePlot(List<GazePoint> gazePoints)
        {
            Polyline polyline = new Polyline();
            foreach (var p in gazePoints)
            {
                Point point = new Point(p.X + Width / 2, p.Y + Height / 2);
                polyline.Points.Add(point);
            }
            polyline.Stroke = new SolidColorBrush(Colors.White);
            polyline.StrokeThickness = 4;
            mainGrid.Children.Add(polyline);
        }


        #endregion

        private List<GazePoint> WriteTestGazePoints(int _xmin = 0, int _xmax = 0, int _ymin = 0, int _ymax = 0,int count=100)
        {

            List<GazePoint> records = new List<GazePoint>();
            Random random = new Random();
            for (int i = 0; i <= count; i++)
            {
                float x = 0 + random.Next(_xmin,_xmax);
                float y = 0 + random.Next(_ymin,_ymax);
                double normX = x / (_MainWindow.Width / 2);
                double normY = y / (_MainWindow.Height / 2);
                //debug
                {
                    //Ellipse ellipse = new Ellipse();
                    //ellipse.MaxHeight = 5;
                    //ellipse.MaxWidth = 5;
                    //ellipse.Fill = new SolidColorBrush(Colors.Black);
                    //ellipse.RenderTransform = new TranslateTransform(x, y);
                    //mainGrid.Children.Add(ellipse);

                }
                GazePoint gazePoint = new GazePoint(normX, normY, 555);
                records.Add(gazePoint);
            }
            return records;
         
        }

        #region InteractiveProcessing
        private void _MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            switch (key)
            {
                case Key.F1:
                    if (mainGrid.Visibility == Visibility.Visible)
                    {
                        mainGrid.Visibility = Visibility.Hidden;
                    }else if (mainGrid.Visibility == Visibility.Hidden)
                    {
                        mainGrid.Visibility = Visibility.Visible;
                    }
                    GazePoints = csvFileReader.GetGazePointsList().Result;
                    renderTransfromHelper.TransformToWindowCoordinates(ref GazePoints, Width, Height);


                    break;

            }
        }

        private void Show_Heat_Map_Click(object sender, RoutedEventArgs e)
        {
            
            var ellipses = heatMapHelper.GetHeatMap(GazePoints, 0.8f, 0.6f, 0.2f);
            DrawHeatMap(ellipses);
        }

        private void Show_all_points_Click(object sender, RoutedEventArgs e)
        {
            DrawGazePoints();
        }

        private void Next_Frame_Click(object sender, RoutedEventArgs e)
        {
       
            
            var color = Color.FromArgb(65, 255, 255, 255);
            if (GazePointIndex > GazePoints.Count)
            {
                GazePointIndex = GazePoints.Count - 1;
            }
            if (GazePointIndex < GazePoints.Count) {
                
                DrawGazePoint(GazePoints[GazePointIndex],color);
                GazePointIndex++;
            }
           

        }

        private void Previous_Frame_Click(object sender, RoutedEventArgs e)
        {
            var transparency = Color.FromArgb(100, 0, 0, 0);

            Debug.WriteLine(GazePointIndex);
            if (GazePointIndex >= 1)
            {
                GazePointIndex--;
            }
        }

        private void Next_GazePlot_Click(object sender, RoutedEventArgs e)
        {
            
            if (GazePointIndex>=2)
            DrawGazePlot(GazePointIndex);
        }

        private void Show_GazePlot_Click(object sender, RoutedEventArgs e)
        {
            DrawGazePlot(GazePoints);
        }
        #endregion
    }
}
