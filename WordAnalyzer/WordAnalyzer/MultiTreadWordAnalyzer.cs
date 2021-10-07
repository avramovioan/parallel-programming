using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WordAnalyzer
{
    class MultiTreadWordAnalyzer
    {
        public void InvokeAsync(string path)
        {
            string[] words = Reader(path);

            List<Thread> threads = new List<Thread>();

            Thread thread1 = new Thread(this.NumOfWords);
            threads.Add(thread1);
            thread1.Start(words);

            Thread thread2 = new Thread(this.AverageWordLength);
            threads.Add(thread2);
            thread2.Start(words);

            Thread thread3 = new Thread(this.TheLongestWord);
            threads.Add(thread3);
            thread3.Start(words);

            Thread thread4 = new Thread(this.TheShortestWord);
            threads.Add(thread4);
            thread4.Start(words);

            Thread thread5 = new Thread(this.FiveMostCommonWords);
            threads.Add(thread5);
            thread5.Start(words);

            Thread thread6 = new Thread(this.FiveLeastCommonWords);
            threads.Add(thread6);
            thread6.Start(words);

            foreach (Thread thread in threads) thread.Join();


        }
        public string[] Split(string text)
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
        private void NumOfWords(object obj)
        {
            string[] words = (string[])obj;
            Console.WriteLine("Number of words: " + words.Length);
        }

        private void TheShortestWord(object obj)
        {
            string[] words = (string[])obj;
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

        private void TheLongestWord(object obj)
        {
            string[] words = (string[])obj;
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
        private void AverageWordLength(object obj)
        {
            string[] words = (string[])obj;
            int sum = 0;
            int count = 0;
            foreach (string word in words)
            {
                sum += word.Length;
                count++;
            }
            Console.WriteLine("Average Word length: " + (count == 0 ? 0 : sum / count));
        }

        private void FiveMostCommonWords(object obj)
        {
            string[] words = (string[])obj;
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

        private void FiveLeastCommonWords(object obj)
        {
            string[] words = (string[])obj;
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
