using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class Call
    {
        public Agent Caller { get; set; }
        public Level LevelTo { get; set; }
        
        public Call(Agent caller, Level levelTo)
        {
            this.Caller = caller;
            this.LevelTo = levelTo;
        }
    }
}
