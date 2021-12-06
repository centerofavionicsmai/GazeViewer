﻿using GazeViewer.Infastructure.Commands;
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
using System.Globalization;
using GazeViewer.Infastructure.Temporarily;
using CsvHelper.Configuration;
using System.IO;
using CsvHelper;

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

        private string _CsvLogsFilePath;
        /// <summary>
        /// Путь к файлу с CSVLogam
        /// </summary>
        public string CsvLogsFilePath
        {
            get => _CsvLogsFilePath;
            set => Set(ref _CsvLogsFilePath, value);
        }

        private string _VideoStreamPath;
        /// <summary>
        /// Путь к файлу с CSVLogam
        /// </summary>
        public string  VideoStreamPath
        {
            get => _VideoStreamPath;
            set => Set(ref _VideoStreamPath, value);
        }

        private ObservableCollection<GazePoint> _CsvFileGazePoints;
        public ObservableCollection<GazePoint> CsvFileGazePoints
        {
            get => _CsvFileGazePoints;
            set => Set(ref _CsvFileGazePoints, value);
        }

        
        private double _CurrentWidth;
        public double CurrentWidth
        {
            get => _CurrentWidth;
            set => Set(ref _CurrentWidth, value);
        }

        private double _CurrentHeight;
        public double CurrentHeight
        {
            get => _CurrentHeight;
            set => Set(ref _CurrentHeight, value);
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
        public ICommand OpenLogFileCommand { get; }
        private bool CanOpenLogFileCommandExecuted(object p) => true;
        private void OpenLogFileCommandExecute(object p)
        {
            _CsvLogsFilePath = DialogService.OpenFileDialog();
          //  ReadCsvLogsCommandExecute(null);
        }


        /// <summary>
        /// Чтение CsvLog`ов  и установка значение GazePoint листа
        /// </summary>
        public ICommand ReadCsvLogsCommand { get; }
        private bool CanReadCsvLogsCommandExecuted(object p)
        {
            if (_CsvLogsFilePath is null) return false;
            return true;
        }
        private void ReadCsvLogsCommandExecute(object p)
        {
            ///Warning in add CollectionMethod
            LogReader logReader = new LogReader();
            GazePoint gazePoint = new GazePoint();
            foreach (var logRecord in logReader.ReadCsvLog(_CsvLogsFilePath))
            {
                var lineData = logRecord.Split(',');
                if (lineData.Length ==3) //Защита от поврежденного файла
                {
                    double x = Double.Parse(lineData[0], CultureInfo.InvariantCulture);
                    double y = Double.Parse(lineData[1], CultureInfo.InvariantCulture);
                    
                    //   double timeStamp = Double.Parse(lineData[2],CultureInfo.InvariantCulture);
                    gazePoint = new GazePoint(x, y, 0);
                }
                _CsvFileGazePoints.Add(gazePoint);
            }
          

        }

   



        #endregion


























       



        public MenuWindowViewModel() {

            OpenLogFileCommand = new ActionCommand(OpenLogFileCommandExecute, CanOpenLogFileCommandExecuted);
            ReadCsvLogsCommand = new ActionCommand(ReadCsvLogsCommandExecute, CanReadCsvLogsCommandExecuted);

            _CsvFileGazePoints = new ObservableCollection<GazePoint>();

            _VideoStreamPath = $@"Video/test.mp4";

            Thread thread = new Thread(new ThreadStart(Test));
            thread.Start();




        }

        private async void Test()
        {



           // UDPController udp = new UDPController(5444);
            CoordConverter coordConverter = new CoordConverter();


            UdpClient udpClient = new UdpClient (5444);


            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            List<GazePoint> gazePoints = new List<GazePoint>(1);
            gazePoints.Add(new GazePoint(0, 0, 0));
            IPEndPoint iPEndPoint = null;
            using (var stream = File.Open(@$"Output\output{DateTime.Now.Millisecond}.csv", FileMode.OpenOrCreate))
            {
                using (var writer = new StreamWriter(stream))
                {



                    using (var csv = new CsvWriter(writer, config))
                    {


                        while (true)
                        {
                            var bytes = udpClient.Receive(ref iPEndPoint);

                            var doubleArray = new double[bytes.Length / 8];
                            Buffer.BlockCopy(bytes, 0, doubleArray, 0, bytes.Length);

                            var coord = (doubleArray[0], doubleArray[1]);

                            //_CsvFileGazePoints.Add(new GazePoint(coord.Item1, coord.Item2, doubleArray[2]));
                            //    gazePoints[0] = new GazePoint(doubleArray[0], doubleArray[1], doubleArray[2]);


                            if (doubleArray[2] != gazePoints[0].TimeStamp  && doubleArray[0] != gazePoints[0].XPoint)
                            {


                                gazePoints[0].XPoint = Xpos;
                                gazePoints[0].YPoint = Ypos;
                                gazePoints[0].TimeStamp = doubleArray[2];
                                csv.WriteRecords(gazePoints);
                            }
                            Xpos = coord.Item1;
                            Ypos = coord.Item2;
                            ReceivedBytes += doubleArray.Length;

                 
                            
                        }
                    }
                }
            }
                
            
        }


        
          
         







        

    


        //private ObservableCollection<GazePoint> _GazePoints;

        //public ObservableCollection<GazePoint> GazePoints
        //{
        //    get => _GazePoints;
        //    set => Set(ref _GazePoints, value);
        //}





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
