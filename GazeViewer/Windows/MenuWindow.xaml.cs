﻿using GazeViewer.Helpers;
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


namespace GazeViewer.Windows
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    
    public partial class MenuWindow : Window
    {
    
        public MenuWindow()
        {
            InitializeComponent();
        }
  


        private async void Media_Loaded(object sender, RoutedEventArgs e)
        {
            Media.RendererOptions.VideoImageType = Unosquare.FFME.Common.VideoRendererImageType.InteropBitmap;
            Media.RendererOptions.UseLegacyAudioOut = true;

            await Media.Open(new Uri("udp://127.0.0.1:5222"));

        }

        private void Media_MediaOpening(object sender, Unosquare.FFME.Common.MediaOpeningEventArgs e)
        {

            e.Options.IsTimeSyncDisabled = true;
            e.Options.IsAudioDisabled = true;
            e.Options.MinimumPlaybackBufferPercent = 0;

            //   e.Options.
            //    e.Options.IsTimeSyncDisabled = false;
       
         


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

        private void GazePoint_MouseDown(object sender, MouseButtonEventArgs e)
        {

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
            if (test2.Stretch == Stretch.None)
            {
                test2.Stretch = Stretch.Uniform;
            }
            else if (test2.Stretch == Stretch.Uniform)
            {
                test2.Stretch = Stretch.None;
            }
        }

        private async void MediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(Properties.videoFilePath);
            if (Properties.videoFilePath !=null)
            await Media.Open(new Uri (Properties.videoFilePath));
           await Media.Play();
        }

        private void MediaElement_Loaded_1(object sender, RoutedEventArgs e)
        {
         //  Debug.WriteLine( media.Source);
            //  Debug.WriteLine(Properties.videoFilePath);
          //  if (Properties.videoFilePath !=null)
         //   media.Source = new Uri(Properties.videoFilePath);
        }

        private async void media_Initialized(object sender, EventArgs e)
        {
        
        }

        private async void media_Loaded_1(object sender, RoutedEventArgs e)
        {



//            string filePath = Properties.videoFilePath;
         //   Debug.WriteLine(filePath);
            string filePath = @"C:\Users\user\Downloads\09.12.21.mp4";
            //  media.so
            //await media.Open(new Uri(filePath));
   
            //Debug.WriteLine(media.Source);
            //media.LoadedBehavior = Unosquare.FFME.Common.MediaPlaybackState.Manual;
            //await media.Play();

        }

        private  async void media_MediaOpening_1(object sender, Unosquare.FFME.Common.MediaOpeningEventArgs e)
        {
            
        }

        private void media_Loaded_2(object sender, RoutedEventArgs e)
        {
            if (Properties.videoFilePath != string.Empty)
            {
                media.LoadedBehavior = MediaState.Manual;
                media.Source = new Uri(Properties.videoFilePath);
                media.Play();
                
            }
        }
    }
}
