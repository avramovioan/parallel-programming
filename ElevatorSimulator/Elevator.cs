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
        private volatile bool DoorOpen = false;
        private Base _base;

        private List<Call> ElevatorCalls = new List<Call>();

        public Elevator(Base b)
        {
            this._base = b;
        }

        public void RegisterCall(Level callLevel, Agent agent = null)
        {
            lock(ElevatorCalls)
            {
                this.ElevatorCalls.Add(new Call(agent, callLevel));
            }
        }
        public void ElevatorWork()
        {
            while (_base.agents_in_base)
            {
                if(ElevatorCalls.Any())
                {
                    Call nextDestination;
                    lock (ElevatorCalls)
                    {
                        nextDestination = this.ElevatorCalls.DefaultIfEmpty(null).FirstOrDefault(c => c.Caller != null) ?? this.ElevatorCalls[0];
                        this.ElevatorCalls.Remove(nextDestination);
                        Console.WriteLine($"Elevator - next stop: {_base.TranslateLevel(nextDestination.LevelTo)}" ); 
                    }
                    if (DoorOpen) Console.WriteLine("The elevator door closes.");
                    this.DoorOpen = false; // close the door in case it's open
                    int floorDifference = Math.Abs((int)nextDestination.LevelTo - (int)CurrentLevel); // calculate difference in floors
                    if (floorDifference > 0) Console.WriteLine("Elevator starts moving.");
                    for (int i = 1; i <= floorDifference; i++)
                    {
                        Thread.Sleep(SPEED); // wait for the elevator to arrive
                        Console.WriteLine("Traveled a floor.");
                    }
                    Console.WriteLine($"The ELEVATOR is at the {_base.TranslateLevel(nextDestination.LevelTo)} floor.");
                    if (nextDestination.Caller == null || SecurityCheck(nextDestination.LevelTo, nextDestination.Caller)) this.DoorOpen = true; // check if agent is eligible
                    this.CurrentLevel = (Level)nextDestination.LevelTo; // set the level to the wanted one
                    Console.WriteLine($"The door opened: {this.DoorOpen}");
                    Thread.Sleep(2000); // wait for the agent to leave/get inside before executing next command
                }
            }     
        }
        public bool Enter(Agent agent)
        {
            semaphore.WaitOne();
            if (this.DoorOpen && agent.currentFloor == this.CurrentLevel)
            {
                return true;
            }
            semaphore.Release();
            return false;
        }

        public bool Leave(Agent agent)
        {
            if (this.DoorOpen)
            {
                semaphore.Release();
                this.DoorOpen = false; //close the door after agent leaves
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
