using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SimpleWordleLib.Helpers;
using SimpleWordleLib.Models;

namespace SimpleWordle
{
    class Program
    {
        static string RawDataSource = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            + @"\Dumps\enwiktionary-latest-all-titles";
        static string JSONDataSource = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            + @"\Dumps\words_dictionary.json";
        static string CleanDataFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) 
            + @"\Dumps\clean_enwiktionary-latest-all-titles";
        static IEnumerable<string> fiveLettersWords = new List<string>();
        static WordleGame theGame = new WordleGame();

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args">n/a</param>
        static void Main(string[] args)
        {
            string myGuess = "";
            //LoadDataFromWiktionary();
            LoadDataFromJSON();

            Console.WriteLine("Simple Wordle " + DateTime.Today.ToString("yyyy-MM-dd") + "!");
            theGame.WordOfTheDay = GetRandomWord().ToLowerInvariant();
            Console.WriteLine();

            while (!theGame.IsCorrect && !theGame.IsTimeOut)
            {
                while (myGuess.Length != 5)
                {
                    myGuess = Console.ReadLine();
                }
                theGame.CheckGuess(myGuess.ToLowerInvariant());
                Console.Write(theGame.Hints[theGame.GuessCount - 1]);
                Console.WriteLine(" " + theGame.GuessCount + @"/6");
                myGuess = "";
            }

            if (theGame.IsCorrect)
            {
                Console.WriteLine();
                Console.WriteLine("Congratulations! You've guessed the right word!");
            }
            if (!theGame.IsCorrect && theGame.IsTimeOut)
            {
                Console.WriteLine();
                Console.WriteLine("Too bad! You've ran out of chance! The correct word is: " + theGame.WordOfTheDay);
            }

            Console.ReadKey();
            return;
        }

        /// <summary>
        /// Get a random 5 characters word from the list.
        /// </summary>
        /// <returns>a 5 characters words for user to guess.</returns>
        private static string GetRandomWord()
        {
            //Random rng = new Random((int)DateTime.Today.Ticks);
            Random rng = new Random((int)DateTime.Now.Ticks);
            int randomIndex = rng.Next(0, fiveLettersWords.Count() - 1);
            return fiveLettersWords.ElementAt(randomIndex);
        }

        /// <summary>
        /// Get clean data of 5 characters words from cleaned wikitionary dump.
        /// </summary>
        /// <param name="Source">wikitionary dump</param>
        private static void LoadCleanData(string Source)
        {
            fiveLettersWords = from words in File.ReadAllLines(Source)
                              where words.Length == 6
                              select words;
        }

        /// <summary>
        /// Get clean data of 5 characters words from JSON file.
        /// </summary>
        private static void LoadDataFromJSON()
        {
            if (File.Exists(JSONDataSource))
            {
                fiveLettersWords = JSONDictionaryLoader.LoadFromJSON(JSONDataSource).Where(o => o.Length == 5);
            }
        }

        /// <summary>
        /// Get clean data of 6 words from dirty wikitionary dump.
        /// </summary>
        private static void LoadDataFromWiktionary()
        {
            if (!File.Exists(CleanDataFile))
            {
                WiktionaryProcessor wikiProcessor = new WiktionaryProcessor();
                wikiProcessor.ProcessRawData(RawDataSource, CleanDataFile);
                fiveLettersWords = wikiProcessor.Words.Where(o => o.Length == 5);
            }
            else
            {
                LoadCleanData(CleanDataFile);
            }

        }
    }
}
