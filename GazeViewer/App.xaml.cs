using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unosquare.FFME;

namespace GazeViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Library.FFmpegDirectory = @"FFmpeg" + (Environment.Is64BitProcess ? @"\x64" : string.Empty);
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            this.Shutdown();
            base.OnExit(e);
        }
    }
}
