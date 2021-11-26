using System;

namespace ElevatorSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            // You can adjust the agent count here (while initializing the base)
            Base b = new Base(3);
            b.OpenBase();
            
        }
    }
}
