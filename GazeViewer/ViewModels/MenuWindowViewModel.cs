using GazeViewer.Infastructure.Commands;
using GazeViewer.Models;
using GazeViewer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Sockets;
using System.Net.WebSockets;
using GazeViewer.Infastructure;
using System.Threading;
using System.Net;
using System.Diagnostics;
using GazeViewer.Helpers;
using System.Windows.Media;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace GazeViewer.ViewModels
{
   internal class MenuWindowViewModel : ViewModel
    {
        private double[,] _Data;


        private OxyPlot.PlotModel _HeatMap;
        public OxyPlot.PlotModel HeatMap
        {
            get => _HeatMap;
            set => Set(ref _HeatMap, value);
        }



        public MenuWindowViewModel() {

            _HeatMap = new PlotModel { Title = "Heatmap" };

            // Color axis (the X and Y axes are generated automatically)
            _HeatMap.Axes.Add(new LinearColorAxis
            {
                Palette = OxyPalettes.Rainbow(100)
            });

            // generate 1d normal distribution
            var singleData = new double[2000];
            for (int x = 0; x < 2000; ++x)
            {
                singleData[x] = Math.Exp((-1.0 / 2.0) * Math.Pow(((double)x - 50.0) / 20.0, 2.0));
            }

            // generate 2d normal distribution
            var data = new double[2000, 2000];
            for (int x = 0; x < 2000; ++x)
            {
                for (int y = 0; y < 2000; ++y)
                {
                    data[x,y] = singleData[x] * singleData[(y + 30) % 100] * 100;
                }
            }

            var heatMapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = 10,
                Y0 = 0,
                Y1 = 10,
                Interpolate = true, 
                RenderMethod = HeatMapRenderMethod.Bitmap, Background = OxyColor.FromArgb(0, 0, 0, 0),
                
                Data = data
            };

            _HeatMap.Series.Add(heatMapSeries);




            //var gazePoints = new ObservableCollection<GazePoint>();
            //for (var x = 0d; x <= 750; x += 1f)
            //{

            //    gazePoints.Add(new GazePoint() { X = x, Y = Math.Pow(Math.Sin(x) * Math.PI / 2, 2) });
            //}
            //_GazePoints = gazePoints;





            Thread thread = new Thread(new ThreadStart(Test));
            thread.Start();



        }

        private async void Test()
        {
            UDPController udp = new UDPController(5555);
            RenderTransfromHelper renderTransfromHelper = new RenderTransfromHelper();
            while (true)
            {
                var bytes = await udp.ReceiveBytesAsync();
              
                var doubleArray = new double[bytes.Length / 8];
                Buffer.BlockCopy(bytes, 0, doubleArray, 0, bytes.Length);

                var coord = (doubleArray[0], doubleArray[1]);
                var normCoord = renderTransfromHelper.TransformToWindow(coord, 800, 450);
             
                Xpos = normCoord.Item1;
                Ypos = normCoord.Item2;
                ReceivedBytes += doubleArray.Length;
            
            }
        }

    


        private ObservableCollection<GazePoint> _GazePoints;

        public ObservableCollection<GazePoint> GazePoints
        {
            get => _GazePoints;
            set => Set(ref _GazePoints, value);
        }

        private double _Xpos;
        public double Xpos
        {
            get => _Xpos;
            set => Set(ref _Xpos, value);
        }

        private double _Ypos;
        public double Ypos
        {
            get => _Ypos;
            set => Set(ref _Ypos, value);
        }




        private double _ReceivedBytes;
        public double ReceivedBytes
        {
            get => _ReceivedBytes;
            set => Set(ref _ReceivedBytes, value);
        }






        private string _Title = "Menu";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #region SliderSettings
        //Int используется специально, так как мы идем по List<GazePoint>
        private int _MaxSliderValue = 900;
        public int MaxSliderValue
        {
            get => _MaxSliderValue;
            set => Set(ref _MaxSliderValue, value);
        }

        private int _MinSliderValue = 0;
        public int MinSliderValue
        {
            get => _MinSliderValue;
            set => Set(ref _MinSliderValue, value);
        }

        private int _SliderValue;
        public int SliderValue
        {
            get => _SliderValue;
            set => Set(ref _SliderValue, value);
        }
        #endregion

        









    }
}
