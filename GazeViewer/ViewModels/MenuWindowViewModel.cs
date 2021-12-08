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

        private int _LogsDelayFilter = 150;
        public  int LogsDelayFilter
        {
            get => _LogsDelayFilter;
            set => Set(ref _LogsDelayFilter, value);
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



        private byte[] UDPBytes = new byte[24];
       






















       



        public MenuWindowViewModel() {

            OpenLogFileCommand = new ActionCommand(OpenLogFileCommandExecute, CanOpenLogFileCommandExecuted);
            ReadCsvLogsCommand = new ActionCommand(ReadCsvLogsCommandExecute, CanReadCsvLogsCommandExecuted);

            _CsvFileGazePoints = new ObservableCollection<GazePoint>();

            _VideoStreamPath = $@"Video/test.mp4";

            Thread vizualizeThread = new Thread(new ThreadStart(VizualizeGazePointThread));
            vizualizeThread.Start();

            Thread writeLogsThread = new Thread(new ThreadStart(WriteLogsThread));
            writeLogsThread.Start();




        }

        private void WriteLogsThread()
        {
                  GazePoint gazePoint = new GazePoint(0, 0, 0);
                  CSVWriter writer = new CSVWriter($"Output/{DateTime.Now.Millisecond}.csv");
                  while (true)
                  { 
                        
                      var doubleArray = new double[3];
                      Buffer.BlockCopy(UDPBytes, 0, doubleArray, 0, UDPBytes.Length);
                        if (doubleArray[2] != gazePoint.TimeStamp)
                        {
                            gazePoint.XPoint = doubleArray[0];
                            gazePoint.YPoint = doubleArray[1];
                            gazePoint.TimeStamp = doubleArray[2];

                            writer.WriteGazePoint(gazePoint);
                        }
                Thread.Sleep(LogsDelayFilter);
                  }        
            }
               
           

        private async void VizualizeGazePointThread()
        {
            UdpClient udpClient = new UdpClient (5444);
            while (true)
            {
                var result = await udpClient.ReceiveAsync();
                UDPBytes = result.Buffer;
                var doubleArray = new double[UDPBytes.Length / 8];
                Buffer.BlockCopy(UDPBytes, 0, doubleArray, 0, UDPBytes.Length);
                Xpos = doubleArray[0];
                Ypos = doubleArray[1];
            }
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
