using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace SimpleWordleLib.Helpers
{
    public class WiktionaryProcessor
    {
        public List<string> Words;
        public WiktionaryProcessor()
        {
            Words = new List<string>();
        }

        /// <summary>
        /// Process wikitionary dump and remove all empty strings and words that have symbols with them.
        /// </summary>
        /// <param name="RawDataSource">Raw File from wikitionary</param>
        /// <param name="CleanDataFile">Final File to use</param>
        public void ProcessRawData(string RawDataSource, string CleanDataFile)
        {
            foreach(string line in File.ReadLines(RawDataSource))
            {
                string word = Regex.Replace(line, @"[\d\t]", "").Trim();
                if (Regex.Match(word, @"^[a-zA-Z]*$").Success && !string.IsNullOrEmpty(word.Trim()))
                {
                    Words.Add(word.Trim().ToLowerInvariant());
                }
            }

            File.WriteAllLines(CleanDataFile, Words.Distinct());
        }
    }
}
