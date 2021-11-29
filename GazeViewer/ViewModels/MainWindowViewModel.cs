using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GazeViewer.Models;
using GazeViewer.ViewModels.Base;
namespace GazeViewer.ViewModels
{
   internal class MainWindowViewModel:ViewModel
    {

        private string _Title = "Gaze Viewer";
            
        public string Title
        {
            get => _Title;
            set=>Set(ref _Title,value);
        }

    }
}
