using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    enum Level { G = 1, S = 2, T1 = 3, T2 = 4 };
    enum SecurityLevel { Confidential = 1, Secret = 2, TopSecret = 4 };
    class Base
    {
        public static Dictionary<Level, int> waitingAgents = new Dictionary<Level, int>();
        public Elevator elevator { get; set; }
        
    }
}
