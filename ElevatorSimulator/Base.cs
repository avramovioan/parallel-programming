using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    enum Level { G = 1, S = 2, T1 = 3, T2 = 4 };
    enum SecurityLevel { Confidential = 1, Secret = 2, TopSecret = 4 };
    class Base
    {
        public Elevator elevator;
        private int agentCount = 0;
        public static bool agents_in_base = false;
        public Base(int count)
        {
            this.elevator = new Elevator();
            this.agentCount = count;
        }
        public void OpenBase()
        {
            var elevator_thread = new Thread(elevator.ElevatorWork);
            agents_in_base = true;
            elevator_thread.Start();
            List<Thread> threads = new List<Thread>();
            //threads.Add(elevator_thread);
            for (int i = 1; i <= agentCount; i++)
            {
                Agent agent = new Agent(i.ToString(), this);
                var thread = new Thread(agent.PaintTheBaseRed);
                thread.Start();
                threads.Add(thread);
            }            
            foreach (var t in threads) t.Join();
            agents_in_base = false;
        }
    }
}
