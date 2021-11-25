using GazeViewer.Infastructure.Commands;
using GazeViewer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GazeViewer.ViewModels
{
   internal class MenuWindowViewModel : ViewModel
    {
        public MenuWindowViewModel()
        {
            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecute,CanCloseApplicationCommandExecuted);
        }



        private string _Title = "Menu";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #region SliderSettings
        //Int используется специально, так как мы идем по List<GazePoint>
        private int _MaxSliderValue = 900;
        public int MaxSliderValue
        {
            get => _MaxSliderValue;
            set => Set(ref _MaxSliderValue, value);
        }

        private int _MinSliderValue = 0;
        public int MinSliderValue
        {
            get => _MinSliderValue;
            set => Set(ref _MinSliderValue, value);
        }

        private int _SliderValue;
        public int SliderValue
        {
            get => _SliderValue;
            set => Set(ref _SliderValue, value);
        }
        #endregion

        #region Commands
            #region CloseApplicationComman
        public ICommand CloseApplicationCommand { get; set; }

        private void OnCloseApplicationCommandExecute(object p)
        {
            Application.Current.Shutdown();
        }
        private bool CanCloseApplicationCommandExecuted(object p)
        {
            return true;
        }
           #endregion


        #endregion







    }
}
