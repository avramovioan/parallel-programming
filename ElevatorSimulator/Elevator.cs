using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class Elevator
    {
        Semaphore semaphore = new Semaphore(1, 1);
        private readonly int SPEED = 1000;
        public Level CurrentLevel = Level.G;
        private bool hasTaskFromAgent = false;
        private bool DoorOpen = false;
        private List<Level> ExternalCalls = new List<Level>();
        //private Level InternalCall;

        public void RegisterExternalCall(Level callLevel)
        {
            lock(ExternalCalls)
            {
                this.ExternalCalls.Add(callLevel);
            }
            if (!hasTaskFromAgent)
            {
                this.hasTaskFromAgent = true;
                TravelToLevel(callLevel);
            }
        }
        public void RegisterInternalCall(Level callLevel, Agent agent)
        {
            TravelToLevel(callLevel, agent);
        }
        public void TravelToLevel(Level destinationLevel, Agent agentInside = null)
        {
            lock (ExternalCalls)
            {
                
            }
            this.DoorOpen = false;
            int floorDifference = Math.Abs((int)destinationLevel - (int)CurrentLevel);
            Thread.Sleep(floorDifference * SPEED);
            this.CurrentLevel = destinationLevel;
            if(agentInside == null || SecurityCheck(destinationLevel, agentInside)) this.DoorOpen = true;
            // add recursive logic to call itself in case it has no job from an agent
        }
        public bool Enter(Agent agent)
        {
            if (this.DoorOpen)
            {
                semaphore.WaitOne();
                return true;
            }
            return false;
        }

        public bool Leave(Agent agent)
        {
            if (DoorOpen)
            {
                semaphore.Release();
                return true;
            }
            return false;
        }

        private bool SecurityCheck(Level level, Agent agent)
        {
            int required_clearance = (int)level;
            int agent_auth = (int)agent.securityLevel;
            if (agent_auth >= required_clearance) return true;
            return false;
        }
    }
    
}
