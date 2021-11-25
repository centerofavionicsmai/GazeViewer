using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvReaderTest
{
    class Program
    {
        static async Task<Stream> GetDataStreamAsync()
        {

            FileStream fileStream = File.Open(@$"test.csv",FileMode.Open);

            return fileStream;
        }


        public IEnumerable<string> GetDataLineAsync()
        {
            using var stream = GetDataStreamAsync().Result;
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLineAsync().Result;
                yield return line;
            }
        }


        static async Task Main(string[] args)
        {
            


       
        }
    }
}
