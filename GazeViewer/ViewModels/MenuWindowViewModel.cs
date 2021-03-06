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
using OxyPlot.Wpf;


namespace GazeViewer.ViewModels
{
    internal class MenuWindowViewModel : ViewModel
    {
        #region Properties
        List<GazePoint> gazePoints = new List<GazePoint>();


        private OxyPlot.PlotModel _PlotModel;
        public OxyPlot.PlotModel PlotModel
        {
            get => _PlotModel;
            set => Set(ref _PlotModel, value);
        }
       

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


        private ObservableCollection<DataPoint> Points;

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

        private int _GazePointSliderValue;
        public int GazePointSliderValue
        {
            get => _GazePointSliderValue;
            set => Set(ref _GazePointSliderValue, value);
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
            string filePath = DialogService.OpenFileDialog();
            if (filePath != null && filePath != string.Empty)
            {
                if (filePath.EndsWith(".csv"))
                {
                    _CsvLogsFilePath = filePath;
                }
                else if (filePath.EndsWith(".avi"))
                {
                    _VideoStreamPath = filePath;
                    Properties.videoFilePath = _VideoStreamPath;
                }
            }
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
        }

        public ICommand GenerateTimeGraphCommand { get; }
        private bool CanGenerateTimeGraphCommandExecuted (object p)
        {

            
                if (gazePoints.Count > 0)
                {
                    return true;
                }
                else return false;
        }
        private void GenerateTimeGraphCommandExecute (object p)
        {
     

            // Weekday axis (horizontal)
            _PlotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,

                // Key used for specifying this axis in the HeatMapSeries
                Key = "WeekdayAxis",

                // Array of Categories (see above), mapped to one of the coordinates of the 2D-data array
                ItemsSource = new[]
                {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
    }
            });

            // Cake type axis (vertical)
            _PlotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = new[]
                {
            "Apple cake",
            "Baumkuchen",
            "Bundt cake",
            "Chocolate cake",
            "Carrot cake"
    }
            });

            // Color axis
            _PlotModel.Axes.Add(new LinearColorAxis
            {
                Palette = OxyPalettes.Hot(200)
            });

            var rand = new Random();
            var data = new double[7, 5];
            for (int x = 0; x < 5; ++x)
            {
                for (int y = 0; y < 7; ++y)
                {
                    data[y, x] = rand.Next(0, 200) * (0.13 * (y + 1));
                }
            }

            var heatMapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = 6,
                Y0 = 0,
                Y1 = 4,
                XAxisKey = "WeekdayAxis",
                YAxisKey = "CakeAxis",
                RenderMethod = HeatMapRenderMethod.Rectangles, FontSize = 12,
                LabelFontSize = 0.2, // neccessary to display the label
                Data = data
            };
           
            _PlotModel.Series.Add(heatMapSeries);



            _PlotModel.InvalidatePlot(true);




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
            gazePoints.Clear();
            CsvFileGazePoints.Clear();
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
            GenerateTimeGraphCommand = new ActionCommand(GenerateTimeGraphCommandExecute, CanGenerateTimeGraphCommandExecuted);
            #endregion
            _PlotModel = new PlotModel();
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
                if (doubleArray[0] != gazePoint.XPoint && doubleArray[1] != gazePoint.YPoint)
                {
                    gazePoint.XPoint = doubleArray[0];
                    gazePoint.YPoint = doubleArray[1];
                    gazePoint.TimeStamp = doubleArray[6];
                    writer.WriteGazePoint(gazePoint);
                }
                
                if (token.IsCancellationRequested)
                {
                    try
                    {
                        process.StandardInput.WriteLine("q");
                    }
                    catch
                    {
                        return;
                    }
             
                }
                Thread.Sleep(LogsDelayFilter);
            }
        }

        private async void VizuaLizeLogsThread(object p)
        {
            DateTime dateTime = new DateTime(1970, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc);
            while (true){
                if (GazePointSliderValue < CsvFileGazePoints.Count)
                {
                    try
                    {
                        TGazePoint = CsvFileGazePoints[GazePointSliderValue];
                      
                        GazePointDateTime = dateTime.AddMilliseconds(CsvFileGazePoints[GazePointSliderValue].TimeStamp).ToLocalTime();

                        double ts = CsvFileGazePoints.Last().TimeStamp - CsvFileGazePoints.First().TimeStamp;//Общее время сессии
                        double currentPost = CsvFileGazePoints.Last().TimeStamp - CsvFileGazePoints[GazePointSliderValue].TimeStamp;

                        SessionTime = TimeSpan.FromMilliseconds(Math.Abs(currentPost - ts));
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
            var doubleArray = new double[sizeof(double)*7 / 8];
            while (true)
            { 
                var result = await udpClient.ReceiveAsync();
                CurrentDataTime = DateTime.Now;
                UDPBytes = result.Buffer;
                
                Buffer.BlockCopy(UDPBytes, 0, doubleArray, 0, UDPBytes.Length);
                Xpos = doubleArray[0];
                Ypos = doubleArray[1];
            }
        }

        private void StartVideoRecording(object p)
        {
            CancellationToken ts = (CancellationToken)p;
          //  string temp = "timestamp\:%{pts\:gmtime\:0\:% H\\\:% M\\\:% S";
            string argument = @"-r 60 -f gdigrab  -i title=VizualizeWindow -vf drawtext=fontfile=/usr/share/fonts/truetype/freefont/TimesNewRoam.ttf:text='%{localtime}':fontcolor=yellow@1:x=2:y=35 -vcodec libx264 -preset ultrafast -draw_mouse 0  -tune zerolatency -threads 8 -thread_type slice" +   $" Video/{DateTime.Now.ToString("yyyy.MM.dd.HH-mm.ss", CultureInfo.InvariantCulture)}.avi";
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
