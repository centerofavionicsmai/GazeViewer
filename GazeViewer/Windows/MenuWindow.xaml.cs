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
         //   Media.RendererOptions.VideoRefreshRateLimit = 1;
            Media.RendererOptions.UseLegacyAudioOut = true;
        
            await Media.Open(new Uri("udp://127.0.0.1:5222"));
           // Debug.WriteLine("аа");
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
    }
}
