using GazeViewer.Helpers;
using GazeViewer.Infastructure;
using GazeViewer.Infastructure.Temporarily;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unosquare.FFME;
using Unosquare;
using System.Windows.Threading;
using GazeViewer.ViewModels;

namespace GazeViewer.Windows
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    
    public partial class MenuWindow : Window
    {

        private bool timeSliderClick;
        DispatcherTimer timerVideoTime = new DispatcherTimer();


        public MenuWindow()
        {
            InitializeComponent();
          
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private TimeSpan TotalTime;

        private void Media_VideoFrameDecoded(object sender, Unosquare.FFME.Common.FrameDecodedEventArgs e)
        {
     
        }

        private void Media_MediaOpening(object sender, Unosquare.FFME.Common.MediaOpeningEventArgs e)
        {
            e.Options.IsTimeSyncDisabled = true;
            e.Options.IsAudioDisabled = true;
            e.Options.MinimumPlaybackBufferPercent = 0;
        } 

        private void deleteme_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (deleteme.Stretch == Stretch.None)
            {
                deleteme.Stretch = Stretch.Uniform;
            } else if (deleteme.Stretch == Stretch.Uniform)
            {
                deleteme.Stretch = Stretch.None;
            }
           
           
        }

        private void Media_MediaInitializing(object sender, Unosquare.FFME.Common.MediaInitializingEventArgs e)
        {
            e.Configuration.ForcedInputFormat = "h264";
            e.Configuration.GlobalOptions.FlagNoBuffer = true;
     
        }

        private void test1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (test1.Stretch == Stretch.None)
            {
                test1.Stretch = Stretch.Uniform;
            }
            else if (test1.Stretch == Stretch.Uniform)
            {
                test1.Stretch = Stretch.None;
            }
        }

        private void test2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (test2.Stretch == Stretch.None)
            //{
            //    test2.Stretch = Stretch.Uniform;
            //}
            //else if (test2.Stretch == Stretch.Uniform)
            //{
            //    test2.Stretch = Stretch.None;
            //}
        }

        private async void staticVideo_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.videoFilePath != string.Empty)
            {
              //  await staticVideo.Open(new Uri(Properties.videoFilePath));
               // await staticVideo.Play();
            }
        }

        private void staticVideo_MediaOpened(object sender, Unosquare.FFME.Common.MediaOpenedEventArgs e)
        {

            TotalTime = staticVideo.NaturalDuration.Value;
            timeSlider.Maximum = staticVideo.NaturalDuration.Value.TotalMilliseconds;
            VideoDurationText.Text = $"{TotalTime} - {timeSlider.Maximum}.msec";
            timerVideoTime.Interval = TimeSpan.FromMilliseconds(10);
            timerVideoTime.Tick += new EventHandler(timer_Tick);
            timerVideoTime.Start();

            timeSlider.AddHandler(MouseDownEvent,
                      new MouseButtonEventHandler(timeSlider_MouseLeftButtonUp),
                      true);
        }

    private async void timeSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        
        staticVideo.Position = TimeSpan.FromMilliseconds(timeSlider.Value);
        timeSlider.Value = staticVideo.Position.TotalMilliseconds;
        timeSliderClick = true;
        timerVideoTime.Interval = TimeSpan.FromSeconds(1);
        }

        void timer_Tick(object sender, EventArgs e)
        {

            if (!timeSliderClick)
            {
                timeSlider.Value = staticVideo.Position.TotalMilliseconds;
                timerVideoTime.Interval = TimeSpan.FromMilliseconds(1);
                return;
            }
            if (timeSliderClick)
            {
                staticVideo.Position = TimeSpan.FromMilliseconds(timeSlider.Value);
                timeSliderClick = false;
                return;
            }
          
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!staticVideo.IsPaused)
                    staticVideo.Pause();
                else
                    staticVideo.Play();
            }
        }
    }
}
