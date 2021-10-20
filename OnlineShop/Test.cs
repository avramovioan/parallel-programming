using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace OnlineShop
{
    class Test
    {
        Shop shop;
        int suppliers, buyers;
        Random rand = new Random();
        public Test(Shop shop, int suppliersCount, int buyersCount)
        {
            this.shop = shop;
            this.suppliers = suppliersCount;
            this.buyers = buyersCount;
        }
        public void StartTest()
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < suppliers; i++)
            {
                Thread t = new Thread(SupplierWorker);
                string threadName = "Thread" + i.ToString();
                t.Start(threadName);
                threads.Add(t);
            }
            for (int i = 0; i < buyers; i++)
            {
                Thread t = new Thread(BuyerWorker);
                string threadName = "Thread" + i.ToString();
                t.Start(threadName);
                threads.Add(t);
            }
            foreach (var t in threads) t.Join();
            Console.WriteLine("Test Completed");
        }
        void SupplierWorker(object obj)
        {
            string threadName = (string)obj;
            int itemsToSupply = rand.Next(1, 5);
            Console.WriteLine($"{threadName} will try to add/stock {itemsToSupply} items");
            while (itemsToSupply != 0)
            {
                int quantity = rand.Next(1, 20);
                bool isNewItem = rand.Next() % 2 == 0;
                if (isNewItem)
                {
                    string name = WordGenerator.GenerateWord();
                    Supplier.Supply(this.shop, name, quantity);
                    Console.WriteLine($"{threadName} added {quantity} of new item {name}");
                }
                else
                {
                    string name = this.shop.getRandomItem();
                    Supplier.Supply(this.shop, name, quantity);
                    Console.WriteLine($"{threadName} added {quantity} of {name}");
                }
                itemsToSupply--;
            }
        }
        void BuyerWorker(object obj)
        {
            string threadName = (string)obj;
            int itemsToBuy = rand.Next(1, 5);
            Console.WriteLine($"{threadName} will try to buy {itemsToBuy} items");
            while (itemsToBuy != 0)
            {
                int quantity = rand.Next(1, 10);
                string name = this.shop.getRandomItem();
                bool success = Buyer.Purchase(this.shop, name, quantity);
                if (success)
                {
                    Console.WriteLine($"{threadName} purchased {quantity} of {name}");
                }
                else
                {
                    Console.WriteLine($"{threadName} wanted to get {quantity} of {name}, but there weren't enough");
                }
                itemsToBuy--;
            }
        }

    }
}
