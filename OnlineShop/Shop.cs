using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OnlineShop
{
    class Shop
    {
        public volatile Dictionary<string, int> items;
        object locker = new object();
        public Shop(Dictionary<string, int> items)
        {
            this.items = items;
        }

        public void Supply(string product, int quantity)
        {
            Monitor.Enter(locker);
            if (items.ContainsKey(product))
            {
                items[product] += quantity;
            }
            else
            {
                items.Add(product, quantity);
            }
            Monitor.Exit(locker);
        }

        public bool Purchase(string product, int quantity)
        {
            Monitor.Enter(locker);
            if (items.ContainsKey(product) && items[product] >= quantity)
            {
                items[product] -= quantity;
                Monitor.Exit(locker);
                return true;
            }
            Monitor.Exit(locker);
            return false;

        }
        public string getRandomItem()
        {
            Random rand = new Random();
            Monitor.Enter(locker);
            string item  = items.ElementAt(rand.Next(1,items.Count)).Key;
            Monitor.Exit(locker);
            return item;
        }

        
    }
}
