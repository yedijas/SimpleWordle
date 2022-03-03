using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWordleLib.Models
{
    public class WordleGame
    {
        #region
        public string WordOfTheDay { get; set; }
        public int GuessCount { get; set; }
        public List<string> GuessWords { get; set; }
        public List<string> Hints { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsTimeOut
        {
            get
            {
                return GuessCount == 6;
            }
        }
        #endregion
        #region
        public WordleGame()
        {
            WordOfTheDay = "";
            GuessCount = 0;
            IsCorrect = false;
            Hints = new List<string>();
            GuessWords = new List<string>();
        }

        public WordleGame(string _wordOfTheDay)
        {
            WordOfTheDay = _wordOfTheDay;
            GuessCount = 0;
            IsCorrect = false;
            Hints = new List<string>();
            GuessWords = new List<string>();
        }

        /// <summary>
        /// Check the guess whether it is correct or not. Set the hints for current guess.
        /// </summary>
        /// <param name="userWord"></param>
        public void CheckGuess(string userWord)
        {
            CheckHints(userWord, WordOfTheDay);
            if (userWord == WordOfTheDay)
                IsCorrect = true;
            else
                IsCorrect = false;
        }

        /// <summary>
        /// Set the hints of each letter from the guess.
        /// </summary>
        /// <param name="userWord">user input/guess word</param>
        /// <param name="WordOfTheDay">correct word for the day</param>
        public void CheckHints(string userWord, string WordOfTheDay)
        {
            GuessCount += 1;
            var uwArray = userWord.ToCharArray();
            var cwArray = WordOfTheDay.ToCharArray();
            Dictionary<int,char> usedIndex = new Dictionary<int, char>();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < uwArray.Length; i++)
            {
                if (uwArray[i] == cwArray[i])
                {
                    sb.Append("V");
                    usedIndex.Add(i,uwArray[i]);
                }
                else if (WordOfTheDay.Contains(uwArray[i]))
                {
                    int startIndex = 0;
                    if (usedIndex.Count > 0)
                    {
                        foreach (KeyValuePair<int, char> single in usedIndex)
                        {
                            if (single.Value == uwArray[i] && single.Key >= startIndex)
                            {
                                startIndex = single.Key + 1;
                            }
                        }
                    }
                    int detectedIndex = WordOfTheDay.IndexOf(uwArray[i], startIndex);
                    while (detectedIndex!= -1 && WordOfTheDay[detectedIndex] == uwArray[detectedIndex])
                    {
                        detectedIndex = WordOfTheDay.IndexOf(uwArray[i], detectedIndex+1);
                    }
                    if (detectedIndex != -1)
                    {
                        sb.Append("M");
                        usedIndex.Add(detectedIndex, uwArray[i]);
                    }
                    else
                    {
                        sb.Append("X");
                    }
                }
                else
                {
                    sb.Append("X");
                }
            }
            Hints.Add(sb.ToString());
        }
        #endregion
    }
}
