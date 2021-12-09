using CsvHelper;
using CsvHelper.Configuration;
using GazeViewer.Extensions;
using GazeViewer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeViewer.Helpers
{
    class CsvLogWriter
    {

        private readonly string filePath;

        public CsvLogWriter (string filePath)
        { 
            this.filePath = filePath;
            File.CreateText(filePath).Close();
        }


        public void WriteGazePoint(GazePoint gazePoint)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again
                HasHeaderRecord = false, 
                
               
            };
            using (var stream = File.Open(filePath,FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
               
                csv.WriteRecord(gazePoint);
                csv.NextRecord();
            }

        }
    }
}
