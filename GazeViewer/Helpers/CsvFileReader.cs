using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper;
using GazeViewer.Extensions;
using System.Globalization;
using System.IO;

namespace GazeViewer.Helpers
{
    class CsvFileReader
    {

        public  async Task< List<GazePoint>> GetGazePointsList()
        {

            string _path = $@"Output/testout.csv";

            List<GazePoint> gazePoints = new List<GazePoint>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again
                HasHeaderRecord = false,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(_path))
            using (var csv = new CsvReader(reader, config))
            {
                gazePoints = csv.GetRecords<GazePoint>().ToList();
            }
            return gazePoints;
        }   




    }
}
