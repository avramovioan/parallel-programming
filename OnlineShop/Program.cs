using System;
using System.Collections.Generic;

namespace OnlineShop
{
    class Program
    {
        static Random rand = new Random();
        static void Main(string[] args)
        {

            Dictionary<string, int> items = GenerateItems(20);
            Shop shop = new Shop(items);
            Test test = new Test(shop, 100, 1000);
            test.StartTest();
            Console.ReadLine();
        }
        static Dictionary<string, int> GenerateItems(int num)
        {
            Dictionary<string, int> items = new Dictionary<string, int>();
            for (int i = 0; i < num; i++)
            { 
                string itemName = WordGenerator.GenerateWord();
                int quantity = rand.Next(1, 20);
                items.Add(itemName, quantity);
            }
            return items;
        }
    }
}
