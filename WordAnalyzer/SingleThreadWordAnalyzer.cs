using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WordAnalyzer
{
    class SingleThreadWordAnalyzer
    {
        public void Invoke(string path)
        {
            string[] words = Reader(path);
            this.NumOfWords(words);
            this.TheShortestWord(words);
            this.TheLongestWord(words);
            this.AverageWordLength(words);
            this.FiveMostCommonWords(words);
            this.FiveLeastCommonWords(words);

        }
        private string[] Split(string text)
        {
            string[] separators = new string[] { " ", "  ", "\t", ".", ",", "-", "=", "\n", "\r", "_" };

            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
        private string[] Reader(string path)
        {
            StreamReader streamReader = new StreamReader(path, true);
            string text = streamReader.ReadToEnd();
            string[] words = Split(text);

            Console.WriteLine("The encoding used was {0}.", streamReader.CurrentEncoding);
            Console.WriteLine();
            return words;
        }
        private void NumOfWords(string[] words)
        {
            Console.WriteLine("Number of words: " + words.Length);
        }

        private void TheShortestWord(string[] words)
        {
            string shortestWord = words[0];
            foreach (string word in words)
            {
                if (word.Length < shortestWord.Length)
                {
                    shortestWord = word;
                }
            }
            Console.WriteLine("Shortest Word: " + shortestWord);

        }

        private void TheLongestWord(string[] words)
        {
            string longestWord = words[0];
            foreach (string word in words)
            {
                if (word.Length > longestWord.Length)
                {
                    longestWord = word;
                }
            }
            Console.WriteLine("Longest Word: " + longestWord);
        }
        private void AverageWordLength(string[] words)
        {
            int sum = 0;
            int count = 0;
            foreach (string word in words)
            {
                sum += word.Length;
                count++;
            }
            Console.WriteLine("Average Word length: " + (count == 0 ? 0 : sum / count));
        }

        private void FiveMostCommonWords(string[] words)
        {
            Dictionary<string, int> wordsRepeats = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (wordsRepeats.ContainsKey(word))
                {
                    wordsRepeats[word]++;
                    continue;
                }
                wordsRepeats.Add(word, 1);
            }
            var sorted = (from kvp in wordsRepeats
                          orderby kvp.Value descending
                          select kvp).Take(5);

            List<string> mcw = new List<string>();
            foreach (var pair in sorted)
            {
                mcw.Add(pair.Key);
            }
            Console.WriteLine("Five Most Common Words: " + string.Join(",", mcw.ToArray()));
        }

        private void FiveLeastCommonWords(string[] words)
        {
            Dictionary<string, int> wordsRepeats = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (wordsRepeats.ContainsKey(word))
                {
                    wordsRepeats[word]++;
                    continue;
                }
                wordsRepeats.Add(word, 1);
            }
            var sorted = (from kvp in wordsRepeats
                          orderby kvp.Value ascending
                          select kvp).Take(5);
            List<string> lcw = new List<string>();
            foreach (var pair in sorted)
            {
                lcw.Add(pair.Key);
            }
            Console.WriteLine("Five Least Common Words: " + string.Join(",", lcw.ToArray()));
        }
    }
}
