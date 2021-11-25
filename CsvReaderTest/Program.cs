using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
namespace CsvReaderTest
{
    class Program
    {
        static async Task<Stream> GetDataStreamAsync()
        {

            FileStream fileStream = File.Open(@$"test.csv", FileMode.Open);

            return fileStream;
        }


        private static IEnumerable<string> GetDataLineAsync()
        {
            using var stream = GetDataStreamAsync().Result;
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLineAsync().Result;
                yield return line;
            }
        }

        private static string[] Xpositions()
        => GetDataLineAsync().First()// string
             .Split(',')
             .Take(1)
             .ToArray();
             
            



        


        static async Task Main(string[] args)
        {

            //foreach (var p in GetDataLineAsync())
            //{
            //    Console.WriteLine(p);
            //}


            var test = Xpositions();
            Console.WriteLine(string.Join("r\n", test));
       
        }
    }
}
