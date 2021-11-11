using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BarSimulator.Bar;

namespace BarSimulator
{
    class Student
    {
        enum NightlifeActivities { Walk, VisitBar, GoHome };
        enum BarActivities { Drink, Dance, Leave };

        Random random = new Random();

        public string Name { get; set; }
        public Bar Bar { get; set; }
        public int Age { get; set; }
        public double Budget { get; set; }
        public bool StaysAtBar = false;

        private NightlifeActivities GetRandomNightlifeActivity()
        {
            int n = random.Next(10);
            if (n < 3) return NightlifeActivities.Walk;
            if (n < 8) return NightlifeActivities.VisitBar;
            return NightlifeActivities.GoHome;
        }

        private BarActivities GetRandomBarActivity()
        {
            int n = random.Next(10);
            if (n < 4) return BarActivities.Dance;
            if (n < 9) return BarActivities.Drink;
            return BarActivities.Leave;
        }

        private void WalkOut()
        {
            Console.WriteLine($"{Name} is walking in the streets.");
            Thread.Sleep(100);
        }

        private void VisitBar()
        {
            Console.WriteLine($"{Name} is getting in the line to enter the bar.");


            Thread.Sleep(100);
            int n = random.Next(10);
            if (n > 7)
            {
                Console.WriteLine($"{Name} decided to leave the queue and go home.");
                return;
            }

            switch (Bar.Enter(this))
            {
                case BarEnterStatus.Closed:
                    Console.WriteLine($"{Name} tried to enter, but the bar is closed");
                    return;
                case BarEnterStatus.Underaged:
                    Console.WriteLine($"{Name} tried to enter, but is underaged");
                    return;
                case BarEnterStatus.Success:
                    EnterBar();
                    Console.WriteLine($"{Name} entered the bar!");
                    break;
            }
            while (StaysAtBar)
            {
                var nextActivity = GetRandomBarActivity();
                switch (nextActivity)
                {
                    case BarActivities.Dance:
                        Console.WriteLine($"{Name} is dancing.");
                        Thread.Sleep(100);
                        break;
                    case BarActivities.Drink:
                        var myEnumMemberCount = Enum.GetNames(typeof(DrinkType)).Length;
                        int randomDrink = random.Next(myEnumMemberCount);
                        DrinkType drink = (DrinkType)randomDrink;
                        if (CanAffordWantedDrink(drink))
                        {
                            if(Bar.PurchaseDrink(drink))
                            {
                                this.Budget -= Bar.GetDrinkPrice(drink);
                                Console.WriteLine($"{Name} bought {drink.ToString()} and he is about to drink it.");
                                Thread.Sleep(100);
                                break;
                            }
                            Console.WriteLine($"{Name} wanted {drink.ToString()} but it's out of stock.");
                        }
                        Console.WriteLine($"{Name} has not enough money for {drink.ToString()}.");
                        Thread.Sleep(100);
                        break;
                    case BarActivities.Leave:
                        Console.WriteLine($"{Name} is leaving the bar.");
                        Bar.Leave(this);
                        LeaveBar();
                        break;
                    default: throw new NotImplementedException();
                }
            }
        }

        public void LeaveBar()
        {
            StaysAtBar = false;
        }
        public void EnterBar()
        {
            StaysAtBar = true;
        }
        public bool CanAffordWantedDrink(DrinkType drinkType)
        {
            return Bar.GetDrinkPrice(drinkType) <= this.Budget ? true : false;
        }
        public void PaintTheTownRed()
        {
            WalkOut();
            bool staysOut = true;
            while (staysOut)
            {
                var nextActivity = GetRandomNightlifeActivity();
                switch (nextActivity)
                {
                    case NightlifeActivities.Walk:
                        WalkOut();
                        break;
                    case NightlifeActivities.VisitBar:
                        VisitBar();
                        staysOut = false;
                        break;
                    case NightlifeActivities.GoHome:
                        staysOut = false;
                        break;
                    default: throw new NotImplementedException();
                }
            }
            Console.WriteLine($"{Name} is going back home.");
        }

        public Student(string name, Bar bar, int age, double budget)
        {
            Name = name;
            Bar = bar;
            Age = age;
            Budget = budget;
        }
    }
}
