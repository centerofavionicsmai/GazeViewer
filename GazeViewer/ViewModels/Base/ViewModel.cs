﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GazeViewer.ViewModels.Base
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName ] string ProperyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ProperyName));
        }

        protected virtual bool Set<T> (ref T field,T value,[CallerMemberName] string PropertyName = null)
        {
            if (Equals(field,value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }



    }
}
