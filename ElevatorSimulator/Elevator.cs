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

        private List<Call> ElevatorCalls = new List<Call>();

        public void RegisterCall(Level callLevel, Agent agent = null)
        {
            lock(ElevatorCalls)
            {
                this.ElevatorCalls.Add(new Call(agent, callLevel));
            }
        }
        public void ElevatorWork()
        {
            while (Base.agents_in_base)
            {
                if(ElevatorCalls.Any())
                {
                    Call nextDestination;
                    lock (ElevatorCalls)
                    {
                        nextDestination = this.ElevatorCalls.DefaultIfEmpty(null).FirstOrDefault(c => c.Caller != null) ?? this.ElevatorCalls[0];
                        this.ElevatorCalls.Remove(nextDestination);
                        Console.WriteLine($"{nextDestination.LevelTo} || {this.ElevatorCalls.Count}" ); 
                    }
                    Console.WriteLine("The elevator door closes.");
                    this.DoorOpen = false; // close the door in case it's open
                    int floorDifference = Math.Abs((int)nextDestination.LevelTo - (int)CurrentLevel); // calculate difference in floors
                    Console.WriteLine("Elevator starts moving.");
                    for (int i = 1; i <= floorDifference; i++)
                    {
                        Thread.Sleep(SPEED); // wait for the elevator to arrive
                        Console.Write("Traveled a floor | ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("The ELEVATOR has arrived at the wanted floor.");
                    if (nextDestination.Caller == null || SecurityCheck(nextDestination.LevelTo, nextDestination.Caller)) this.DoorOpen = true; // check if agent is eligible
                    this.CurrentLevel = (Level)nextDestination.LevelTo; // set the level to the wanted one
                    Console.WriteLine($"Door open on level {this.CurrentLevel}: " + this.DoorOpen);
                    Thread.Sleep(2000); // wait for the agent to leave/get inside before executing next command
                }
            }     
        }
        public bool Enter(Agent agent)
        {
            //Console.WriteLine($"{agent.Name} - {agent.currentFloor} | Elevator - {this.CurrentLevel} | Door - {this.DoorOpen} | agent_floor - {agent.currentFloor} | el_floor - {this.CurrentLevel}");
            semaphore.WaitOne();
            if (this.DoorOpen && agent.currentFloor == this.CurrentLevel)
            {
                Console.WriteLine($"{agent.Name} entered");
                return true;
            }
            semaphore.Release();
            return false;
        }

        public bool Leave(Agent agent)
        {
            //Console.WriteLine($"{agent.Name} wants to leave");
            if (this.DoorOpen)
            {
                //Console.WriteLine($"{agent.Name} wants to leave");
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
