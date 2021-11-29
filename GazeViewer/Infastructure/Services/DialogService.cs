using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GazeViewer.Infastructure.Services.Interfaces;
using Microsoft.Win32;

namespace GazeViewer.Infastructure.Services
{
    class DialogService : IDialogService
    {
        public string OpenFileDialog()
        {
            string filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = ("CsvFiles (*.csv)|*.csv"),
                InitialDirectory = Environment.CurrentDirectory,
                Title = "Открыть файл"


            };

            if (openFileDialog.ShowDialog() == true)
            {

                filePath = openFileDialog.FileName;

            }
            return filePath;


        }
    }

}
