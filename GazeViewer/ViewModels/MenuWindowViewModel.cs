using GazeViewer.Infastructure.Commands;
using GazeViewer.Models;
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
        public MenuWindowViewModel() {

            var gazePoints = new List<GazePoint>((int)((int) 360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1f)
            {
                gazePoints.Add(new GazePoint { X = x, Y = (double) Math.Sin(x * MathF.PI) });
            }
            _GazePoints = gazePoints;
        }

        private IEnumerable<GazePoint> _GazePoints;

        public IEnumerable<GazePoint> GazePoints
        {
            get => _GazePoints;
            set => Set(ref _GazePoints, GazePoints);
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

        









    }
}
