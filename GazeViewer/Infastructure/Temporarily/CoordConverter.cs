using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeViewer.Infastructure.Temporarily
{
    class CoordConverter
    {
        public double ConvertNormToLocal(double pos,double koef)
        {
            //Debug.WriteLine(pos* (koef/2));
            return pos * (koef / 2);

        }


    }
}
