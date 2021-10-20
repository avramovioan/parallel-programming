using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop
{
    class WordGenerator
    {
        public static string GenerateWord()
        {
            Random rand = new Random();
            int wordLenght = rand.Next(5, 7);
            string word = string.Empty;
            string[] consonants = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z" };
            string[] vowels = new string[] { "a", "e", "i", "o", "u", "y" };
            for (int i = 1; i <= wordLenght; i++)
            {
                word += i % 2 == 0 ? vowels[rand.Next(0, vowels.Length - 1)] : consonants[rand.Next(0, consonants.Length - 1)];
            }
            return word;
        }
    }
}
