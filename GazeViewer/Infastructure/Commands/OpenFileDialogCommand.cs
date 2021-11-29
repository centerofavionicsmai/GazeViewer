using GazeViewer.Infastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using GazeViewer.Infastructure.Services;

namespace GazeViewer.Infastructure.Commands
{
    internal class OpenFileDialogCommand : Command
    {
        
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        { 
         
        }
    }
}
