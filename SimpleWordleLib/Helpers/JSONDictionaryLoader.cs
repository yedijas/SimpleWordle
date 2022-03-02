using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimpleWordleLib.Helpers
{
    public class JSONDictionaryLoader
    {
        /// <summary>
        /// Load data from JSON. This is using Newtonsoft.Json library.
        /// </summary>
        /// <param name="jsonInput">file input to load.</param>
        /// <returns>list of string from json file.</returns>
        static public IEnumerable<string> LoadFromJSON(string jsonInput)
        {
            string jsonString = File.ReadAllText(jsonInput);
            IEnumerable<string> result = JsonConvert.DeserializeObject<List<String>>(jsonString);

            return result;
        }
    }
}
