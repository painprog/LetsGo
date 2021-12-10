using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Subsidiary
{
    public class JsonReader
    {
        public static List<T> ReadJson<T>(string filepath)
        {
            try
            {
                string objects = File.ReadAllText(filepath);
                var bruh = JsonConvert.DeserializeObject<List<T>>(objects);
                return JsonConvert.DeserializeObject<List<T>>(objects);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Can't find a file on {filepath}");
                Console.WriteLine(ex.Message);
                return new List<T>();
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Can't find a file on {filepath}");
                Console.WriteLine(ex.Message);
                return new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<T>();
            }
        }
    }
}
