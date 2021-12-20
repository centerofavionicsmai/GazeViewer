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
            test.Fill = visualBrush;
        }

    }
}
