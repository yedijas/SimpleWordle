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
        static string RawDataSource = @"D:\Projects\data\enwiktionary-latest-all-titles";
        static string CleanDataFile = @"D:\Projects\data\clean_enwiktionary-latest-all-titles";
        static IEnumerable<string> sixLettersWords = new List<string>();
        static WordleGame theGame = new WordleGame();

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args">n/a</param>
        static void Main(string[] args)
        {
            LoadDataFromWiktionary();
            string myGuess = "";

            Console.WriteLine("Simple Wordle " + DateTime.Today.ToString("yyyy-MM-dd") + "!");
            theGame.WordOfTheDay = GetRandomWord();

            while (!theGame.IsCorrect && !theGame.IsTimeOut)
            {
                while (myGuess.Length != 6)
                {
                    myGuess = Console.ReadLine();
                }
                theGame.CheckGuess(myGuess);
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
        /// Get clean data of 6 words from cleaned wikitionary dump.
        /// </summary>
        /// <param name="Source">wikitionary dump</param>
        private static void LoadCleanData(string Source)
        {
            sixLettersWords = from words in File.ReadAllLines(Source)
                              where words.Length == 6
                              select words;
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
                sixLettersWords = wikiProcessor.Words.Where(o => o.Length == 6).ToList();
            }
            else
            {
                LoadCleanData(CleanDataFile);
            }

        }
    }
}
