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
using GazeViewer.Infastructure.Services;
using GazeViewer.Infastructure.Services.Interfaces;
using GazeViewer.Infastructure.Helpers;

namespace GazeViewer.ViewModels
{
   internal class MenuWindowViewModel : ViewModel
    {
        #region Properties
        private double _ReceivedBytes;
        /// <summary>
        /// Количество получанных byte, для Live Версии
        /// </summary>
        public double ReceivedBytes
        {
            get => _ReceivedBytes;
            set => Set(ref _ReceivedBytes, value);
        }

        private string _Title = "Menu";
        /// <summary>
        /// Заголовок окна
        /// </summary>
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

        #endregion

        #region Services
        IDialogService DialogService = new DialogService();
        #endregion


        #region Commands 
        /// <summary>
        /// Открыть файл
        /// </summary>
        public ICommand OpenFileCommand { get; }
        private bool CanOpenFileCommandExecuted(object p) => true;
        private void OpenFileCommandExecute(object p)
        {
            FilePath = DialogService.OpenFileDialog();

        }


        public ICommand ReadCsvLogsCommand { get; }
        private bool CanReadCsvLogsCommandExecuted(object p)
        {
            if (FilePath is null) return false;
            return true;
        }
        private void ReadCsvLogsCommandExecute(object p)
        {
            ///Warning in add CollectionMethod
            LogReader logReader = new LogReader();
            foreach (var logs in logReader.ReadCsvLog(FilePath))
            {
                _Strings.Add(logs);
            }

        }





        #endregion

























        private double[,] _Data;
        private OxyPlot.PlotModel _HeatMap;
        public OxyPlot.PlotModel HeatMap
        {
            get => _HeatMap;
            set => Set(ref _HeatMap, value);
        }


        private string _FilePath;
        public string FilePath
        {
            get => _FilePath;
            set => Set(ref _FilePath, value);
        }



        public MenuWindowViewModel() {

            OpenFileCommand = new ActionCommand(OpenFileCommandExecute, CanOpenFileCommandExecuted);
            ReadCsvLogsCommand = new ActionCommand(ReadCsvLogsCommandExecute, CanReadCsvLogsCommandExecuted);


            _Strings = new ObservableCollection<string>();





            //var gazePoints = new ObservableCollection<GazePoint>();
            //for (var x = 0d; x <= 750; x += 1f)
            //{

            //    gazePoints.Add(new GazePoint() { X = x, Y = Math.Pow(Math.Sin(x) * Math.PI / 2, 2) });
            //}
            //_GazePoints = gazePoints;







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





        private ObservableCollection <string> _Strings;
        public ObservableCollection<string> Strings
        {
            get => _Strings;
            set => Set(ref _Strings, value);
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
















        
        

        









    }
}
