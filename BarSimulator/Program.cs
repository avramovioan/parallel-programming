using System;
using System.Collections.Generic;
using System.Threading;

namespace BarSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            Bar bar = new Bar();
            List<Thread> studentThreads = new List<Thread>();
            for (int i = 1; i < 100; i++)
            {
                var student = new Student(i.ToString(), bar, rand.Next(16,50), rand.NextDouble()*100);
                var thread = new Thread(student.PaintTheTownRed);
                thread.Start();
                studentThreads.Add(thread);
            }

            foreach (var t in studentThreads) t.Join();
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            bar.PrintIncome();
            Console.ReadLine();
        }
    }
}
