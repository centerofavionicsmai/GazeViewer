using CsvHelper;
using CsvHelper.Configuration;
using GazeViewer.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeViewer.Helpers
{
    class CSVWriter
    {

        public void WriteGazePoints(List<GazePoint> gazePoints)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again
                HasHeaderRecord = false,
            };

            using (var writer = new StreamWriter(@"Output/testout.csv"))
            using (var csv = new CsvWriter(writer, config))
            {
               csv.WriteRecords(gazePoints);
            }
        }
    }
}
