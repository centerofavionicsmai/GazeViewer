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
using GazeViewer.Windows;
using CsvHelper;
using System.Media;
using System.Windows.Controls;
using Unosquare.FFME;

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
        VizualizeWindow VizualizeWindow = new VizualizeWindow();
        private byte[] UDPBytes = new byte[24];

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
        public string VideoStreamPath
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

        private int _LogsDelayFilter;
        public int LogsDelayFilter
        {
            get => _LogsDelayFilter;
            set => Set(ref _LogsDelayFilter, value);
        }

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private double _Xpos;
        public double Xpos
        {
            get => _Xpos;
            set => Set(ref _Xpos, value);
        }

        private Unosquare.FFME.MediaElement _MediaElement;
        public Unosquare.FFME.MediaElement MediaElement
        {
            get => _MediaElement;
            set => Set(ref _MediaElement, value);
        }





        private double _Ypos;
        public double Ypos
        {
            get => _Ypos;
            set => Set(ref _Ypos, value);
        }

        private TimeSpan _VideoTs = TimeSpan.FromSeconds(3);
        public TimeSpan VideoTs
        {
            get => _VideoTs;
        }


        private System.Windows.Data.Binding _Binding;
        public System.Windows.Data.Binding Binding
        {
            get => _Binding;

            set => Set(ref _Binding, value);
        }

        #region SliderSettings
        //Int используется специально, так как мы идем по List<GazePoint>
        private double _MaxSliderValue;
        public double MaxSliderValue
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

        private PointCollection _GazePlot;
        public PointCollection GazePlot
        {
            get => _GazePlot;

            set => Set(ref _GazePlot, value);

        }



        private int _SliderValue;
        public int SliderValue
        {
            get => _SliderValue;
            set => Set(ref _SliderValue, value);
        }

        private GazePoint _TGazePoint;
        public GazePoint TGazePoint
        {
            get {
                return _TGazePoint;
            }
            set => Set(ref _TGazePoint, value);
        }

        private DateTime _GazePointDateTime;
        public DateTime GazePointDateTime
        {
            get => _GazePointDateTime;
            set => Set(ref _GazePointDateTime, value);

        }

        private TimeSpan _SessionTime;
        public TimeSpan SessionTime
        {
            get => _SessionTime;
            set => Set(ref _SessionTime, value);
        }


        private DateTime _CurrentDataTime;
        public DateTime CurrentDataTime
        {
            get => _CurrentDataTime;
            set => Set(ref _CurrentDataTime, value);
        }

        private byte _HetMapIntensivity = 5;
        public byte HeatMapIntensivity
        {
            get => _HetMapIntensivity;
            set => Set(ref _HetMapIntensivity, value);
        }
        Process process;

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
            if (p.ToString() == "videofilepath")
            {

                _VideoStreamPath = DialogService.OpenFileDialog();
                //   Debug.WriteLine(_VideoStreamPath);
                Properties.videoFilePath = _VideoStreamPath;
            }
            else if (p.ToString() == "logsfilepath")
                _CsvLogsFilePath = DialogService.OpenFileDialog();
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

            List<GazePoint> gazePoints = new List<GazePoint>();
            int i = 0;
            foreach (var logRecord in logReader.ReadCsvLog(_CsvLogsFilePath))
            {
             
                var lineData = logRecord.Split(',');
        
                if (lineData.Length == 3) //Защита от поврежденного файла
                {
                  
                    double x = Double.Parse(lineData[0], CultureInfo.InvariantCulture);
                    double y = Double.Parse(lineData[1], CultureInfo.InvariantCulture);
                    double timeStamp = Double.Parse(lineData[2], CultureInfo.InvariantCulture);

                    gazePoints.Add(new GazePoint(x, y, Color.FromArgb(_HetMapIntensivity, 88, 171, 232), timeStamp));

                }

            }
         
            CsvFileGazePoints = new ObservableCollection<GazePoint>(gazePoints);
            double duration =   CsvFileGazePoints.Last().TimeStamp - CsvFileGazePoints.First().TimeStamp;
            MaxSliderValue = CsvFileGazePoints.Count;
            MinSliderValue = 0;
        }

        public ICommand ClearGazePointListCommand { get; }
        private bool CanClearGazePointListCommandExecuted(object p)
        {
            if (_CsvFileGazePoints.Count > 0)
            {
                return true;
            }
            else return false;
        }
        private void ClearGazePointListCommandExecute(object p)
        {
            _CsvFileGazePoints.Clear();
        }

        public ICommand StartWriteLogsCommand { get; }
        private bool CanStartWriteLogsCommandExecuted(object p)
        {
            return true;

        }
        private void StartWriteLogsCommandExecute(object p)
        {
            cancellationTokenSource = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback(WriteLogs), cancellationTokenSource.Token);
            var  cancellationTokenSource1 = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartVideoRecording), cancellationTokenSource.Token);

        }

        public ICommand StopWriteLogsCommand { get; }
        private bool CanStopWriteLogsCommandExecuted(object p)
        {

            return cancellationTokenSource.Token.CanBeCanceled;
        }
        private void StopWriteLogsCommandExecute(object p)
        {
            cancellationTokenSource.Cancel();
        }

        public ICommand OpenVizualizeWindowCommand { get; }
        private bool CanOpenVizualizeWindowCommandExecuted (object p)
        {
            if (VizualizeWindow.IsActive)
            {
                return false;
            }
            else return true;
        }
        private void OpenVizualizeWindowCommandExecute(object p)
        {
            VizualizeWindow.DataContext = this;
            VizualizeWindow.Show();
        }
        public ICommand CloseVizualizeWindowCommand { get; }
        private bool CanCloseVizualizeWindowCommandExecuted(object p)
        {
            if (VizualizeWindow.IsLoaded)
            {
                return true;
            }
            else return false;
        }
        private void CloseVizualizeWindowCommandExecute(object p)
        {
            VizualizeWindow.Hide();
        }

        #endregion

        public MenuWindowViewModel() {
            #region RegisterCommands
            OpenLogFileCommand = new ActionCommand(OpenLogFileCommandExecute, CanOpenLogFileCommandExecuted);
            ReadCsvLogsCommand = new ActionCommand(ReadCsvLogsCommandExecute, CanReadCsvLogsCommandExecuted);
            ClearGazePointListCommand = new ActionCommand(ClearGazePointListCommandExecute, CanClearGazePointListCommandExecuted);
            StartWriteLogsCommand = new ActionCommand(StartWriteLogsCommandExecute, CanStartWriteLogsCommandExecuted);
            StopWriteLogsCommand = new ActionCommand(StopWriteLogsCommandExecute, CanStopWriteLogsCommandExecuted);
            OpenVizualizeWindowCommand = new ActionCommand(OpenVizualizeWindowCommandExecute, CanOpenVizualizeWindowCommandExecuted);
            CloseVizualizeWindowCommand = new ActionCommand(CloseVizualizeWindowCommandExecute, CanCloseVizualizeWindowCommandExecuted);
               //   StartVideoRecordingCommand = new ActionCommand(StartVideoRecordingCommandExecute, CanStartVideoRecordingCommandExecuted);
            #endregion

               _CsvFileGazePoints = new ObservableCollection<GazePoint>();
            GazePlot = new PointCollection();

            ThreadPool.QueueUserWorkItem(new WaitCallback(VizualizeGazePointThread));
            ThreadPool.QueueUserWorkItem(new WaitCallback(VizuaLizeLogsThread));
        }

        private void WriteLogs(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            GazePoint gazePoint = new GazePoint(0, 0, Colors.Blue, 0);
            CsvLogWriter writer = new CsvLogWriter($"Output/{DateTime.Now.ToString("yyyy.MM.dd HH-mm .ss", CultureInfo.InvariantCulture)}.csv");

            while (true)
            {
                    var doubleArray = new double[7];
                    Buffer.BlockCopy(UDPBytes, 0, doubleArray, 0, UDPBytes.Length);
                    gazePoint.XPoint = doubleArray[4];
                    gazePoint.YPoint = doubleArray[5];
                    gazePoint.TimeStamp = doubleArray[6];
                writer.WriteGazePoint(gazePoint);
                
                if (token.IsCancellationRequested)
                {
                    process.StandardInput.WriteLine("q");
                    return;
                }
                Thread.Sleep(LogsDelayFilter);
            }
        }

        private async void VizuaLizeLogsThread(object p)
        {
            while (true){
                if (SliderValue < CsvFileGazePoints.Count)
                {
                    try
                    {
                        TGazePoint = CsvFileGazePoints[SliderValue];
                        DateTime dateTime = new DateTime(1970, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc);
                        GazePointDateTime = dateTime.AddSeconds(CsvFileGazePoints[SliderValue].TimeStamp).ToLocalTime();

                        double ts = CsvFileGazePoints.Last().TimeStamp - CsvFileGazePoints.First().TimeStamp;//Общее время сессии
                        double currentPost = CsvFileGazePoints.Last().TimeStamp - CsvFileGazePoints[SliderValue].TimeStamp;

                        SessionTime = TimeSpan.FromSeconds(Math.Abs(currentPost - ts));
                    }
                    catch
                    {

                    }
                }

            }
        }


        private async void VizualizeGazePointThread(object p)
        {

            UdpClient udpClient = new UdpClient(5444);
            while (true)
            { 
                var result = await udpClient.ReceiveAsync();
                CurrentDataTime = DateTime.Now;
                UDPBytes = result.Buffer;
                var doubleArray = new double[UDPBytes.Length / 8];
                Buffer.BlockCopy(UDPBytes, 0, doubleArray, 0, UDPBytes.Length);
                Xpos = doubleArray[0];
                Ypos = doubleArray[1];
            }
        }

        private void StartVideoRecording(object p)
        {
            CancellationToken ts = (CancellationToken)p;
            string argument = @$"-r 60 -f gdigrab  -i title=VizualizeWindow  -vcodec libx264 -preset ultrafast -draw_mouse 0  -tune zerolatency -threads 8 -thread_type slice Video/{DateTime.Now.ToString("yyyy.MM.dd.HH-mm.ss", CultureInfo.InvariantCulture)}.avi";
            using (process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = @"FFmpeg\x64\ffmpeg.exe";
                process.StartInfo.Arguments = argument;
                process.Start();
                process.WaitForExit();

            }

        }




    }
}
