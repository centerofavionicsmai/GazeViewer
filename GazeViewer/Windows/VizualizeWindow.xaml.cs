using GazeViewer.Infastructure;
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
using System.Windows.Shapes;

namespace GazeViewer.Windows
{
    /// <summary>
    /// Interaction logic for VizualizeWindow.xaml
    /// </summary>
    public partial class VizualizeWindow : Window
    {
        public VizualizeWindow(VisualBrush visualBrush)
        {
            InitializeComponent();
           
        }

        private async void Media_Loaded(object sender, RoutedEventArgs e)
        {
            Media.RendererOptions.VideoImageType = Unosquare.FFME.Common.VideoRendererImageType.InteropBitmap;
            Media.RendererOptions.UseLegacyAudioOut = true;
            //Media.VideoFrameDecoded += Media_VideoFrameDecoded;
              await Media.Open(new Uri("udp://127.0.0.1:5222"));
        }

        private void Media_MediaOpening(object sender, Unosquare.FFME.Common.MediaOpeningEventArgs e)
        {
            e.Options.IsTimeSyncDisabled = true;
            e.Options.IsAudioDisabled = true;
            e.Options.MinimumPlaybackBufferPercent = 0;
        }

        private void Media_MediaOpened(object sender, Unosquare.FFME.Common.MediaOpenedEventArgs e)
        {

        }

      
    }
}
