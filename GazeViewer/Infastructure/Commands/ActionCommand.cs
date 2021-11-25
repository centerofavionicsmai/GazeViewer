using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GazeViewer.Infastructure.Commands.Base;
namespace GazeViewer.Infastructure.Commands
{
   internal class ActionCommand : Command
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public ActionCommand(Action<object> Execute,Func<object,bool> CanExecute = null)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            canExecute = CanExecute;
        }

        public override bool CanExecute(object parameter)
        {
          return  canExecute?.Invoke(parameter) ?? true; 
        }

        public override void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
