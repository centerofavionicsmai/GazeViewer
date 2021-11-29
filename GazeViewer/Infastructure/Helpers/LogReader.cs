using GazeViewer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeViewer.Infastructure.Helpers
{
    class LogReader
    {

        public  List<String> ReadCsvLog (string filePath)
        {
            var fileStream = GetFileStream(filePath);
            using var reader = new StreamReader(fileStream);
            List<string> csvString = new List<string>();
            while (!reader.EndOfStream)
            {
               
                var line = reader.ReadLine();
                csvString.Add(line);
            }
            return csvString;
        }

        //private IEnumerable <string> GetCsvString(StreamReader reader)
        //{
        //    while (!reader.EndOfStream)
        //    {
        //        var line = await reader.ReadLineAsync();

        //        yield return line;
        //    }
        //}


        private Stream  GetFileStream(string Path)
        {
            FileStream fileStream = File.Open(Path, FileMode.Open);
            return fileStream;
        }
    }
}
