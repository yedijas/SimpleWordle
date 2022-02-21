using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SimpleWordle.Helper;

namespace SimpleWordle
{
    class Program
    {
        static string RawDataSource = @"D:\Projects\data\enwiktionary-latest-all-titles";
        static string CleanDataFile = @"D:\Projects\data\clean_enwiktionary-latest-all-titles";
        static IEnumerable<string> sixLettersWords = new List<string>();

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args">n/a</param>
        static void Main(string[] args)
        {
            bool guessed = false;
            int counter = 1;
            bool passed = false;

            if (!File.Exists(CleanDataFile))
            {
                WiktionaryProcessor wikiProcessor = new WiktionaryProcessor();
                wikiProcessor.ProcessRawData(RawDataSource, CleanDataFile);
                sixLettersWords = wikiProcessor.Words.Where(o => o.Length == 6).ToList();
            }
            else
            {
                LoadCleanData(CleanDataFile);
            }

            string wordOfTheDay = GetRandomWord();
            string myGuess = "";

            Console.WriteLine("Simple Wordle " + DateTime.Today.ToString("yyyy-MM-dd") + "!");

            while (!guessed && !passed)
            {
                while (myGuess.Length != 6)
                {
                    myGuess = Console.ReadLine();
                }
                if (CheckGuess(myGuess.ToLowerInvariant(), wordOfTheDay))
                {
                    Console.Write(" " + counter + @"/6");
                    guessed = true;
                }
                else
                {
                    Console.Write(" " + counter + @"/6");
                    Console.WriteLine();
                    counter += 1;
                    if (counter == 6)
                    {
                        passed = true;
                    }
                }
                myGuess = "";
            }
            Console.ReadKey();
            return;
        }

        /// <summary>
        /// Check user input guess. Write V if in correct position, M if in wrong position, X if letter not exists.
        /// </summary>
        /// <param name="userWord">user guess</param>
        /// <param name="correctWord">word of the day</param>
        /// <returns></returns>
        private static bool CheckGuess(string userWord, string correctWord)
        {
            var uwArray = userWord.ToCharArray();
            var cwArray = correctWord.ToCharArray();

            for (int i = 0; i < uwArray.Length; i++)
            {
                if (uwArray[i] == cwArray[i])
                {
                    Console.Write("V");
                }
                else if (correctWord.Contains(uwArray[i]))
                {
                    Console.Write("M");
                }
                else
                {
                    Console.Write("X");
                }
            }

            if (userWord == correctWord)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get a random 6 characters word from the list.
        /// </summary>
        /// <returns>a 6 character words for user to guess.</returns>
        private static string GetRandomWord()
        {
            Random rng = new Random((int)DateTime.Today.Ticks);
            int randomIndex = rng.Next(0, sixLettersWords.Count() - 1);
            return sixLettersWords.ElementAt(randomIndex);
        }

        /// <summary>
        /// Get clean data of 6 words from wikitionary dump.
        /// </summary>
        /// <param name="Source">wikitionary dump</param>
        private static void LoadCleanData(string Source)
        {
            sixLettersWords = from words in File.ReadAllLines(Source)
                              where words.Length == 6
                              select words;
        }
    }
}
