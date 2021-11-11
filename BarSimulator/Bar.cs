using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace BarSimulator
{
    class Bar
    {
        public enum DrinkType {Vodka, Whiskey, Brandy, Vermouth, Cognac, Beer, Wine, Rum, Gin };
        public enum BarEnterStatus { Success, Underaged, Closed};
        List<Drink> drinks = new List<Drink>();
        List<Student> students = new List<Student>();
        Semaphore semaphore = new Semaphore(10, 10);
        Random rand = new Random();
        public bool isOpen { get; set; } = true;
        object barlocker = new object();
        public Bar()
        {
            foreach (string drink in Enum.GetNames(typeof(DrinkType)))
            {
                Drink dr = new Drink();
                dr.Name = drink;
                dr.Quantity = rand.Next(15, 20);
                dr.Price = rand.NextDouble() * 8.0;
                drinks.Add(dr);
            }
        }
        public double GetDrinkPrice(DrinkType drink)
        {
            lock (drinks)
            {
                Drink dr = drinks.DefaultIfEmpty(null).FirstOrDefault(d => d.Name == drink.ToString());
                return dr == null ? double.MaxValue : dr.Price;
            }
        }
        public bool PurchaseDrink(DrinkType drink)
        {
            string dr = drink.ToString();
            lock (drinks)
            {
                Drink _drink = drinks.DefaultIfEmpty(null).FirstOrDefault(d => d.Name == dr);
                if (_drink == null || _drink.Quantity == _drink.SoldQunatity) return false;
                
                _drink.SoldQunatity++;
                return true;
            }
            
        }
        public void Close()
        {
            Console.WriteLine($"####### {students.Count} people will be kicked from the bar ########");
            lock (students)
            {
                foreach (Student student in students)
                {
                    
                    Console.WriteLine($"{student.Name} is getting kicked out of the bar.");
                    student.LeaveBar();
                    semaphore.Release();
                }
                students.Clear();
            }
            Console.WriteLine("------THE BAR CLOSED------");
        }
        public void PrintIncome()
        {
            foreach (Drink drink in drinks)
            {
                string stock = drink.Quantity == drink.SoldQunatity ? "out of stock" : $"{drink.Quantity-drink.SoldQunatity} in stock";
                double earned = drink.SoldQunatity * drink.Price;
                Console.WriteLine($"{drink.Name} : {stock}. Sold {drink.SoldQunatity} -- earned: {earned.ToString("0.##")}");
            }
        }

        public bool isBarOpen()
        {
            return isOpen;
        }

        public BarEnterStatus Enter(Student student)
        {
            if (!isOpen) return BarEnterStatus.Closed;
            lock (barlocker)
            {
                BarStaysOpen();
            }
            semaphore.WaitOne();
            lock (students)
            {
                if (!isOpen) return BarEnterStatus.Closed;
                if (student.Age < 18) return BarEnterStatus.Underaged;
                students.Add(student);
                return BarEnterStatus.Success;
            }
        }

        public void Leave(Student student)
        {
            lock (students)
            {
                students.Remove(student);
            }
            semaphore.Release();
        }
        
        public void BarStaysOpen()
        {
            int n = rand.Next(30);
            if (n == 11 && isOpen)
            {
                Console.WriteLine("---------The bar will close in 10 min-------------");
                isOpen = false;
                Thread.Sleep(100);
                Close();
            }
        }
    }
}
