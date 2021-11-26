using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    enum AgentActivity { Wander, CallElevator, Leave };
    enum Status { WaitElevator, InElevator, OutElevator };
    class Agent
    {
        public SecurityLevel securityLevel;
        public Level currentFloor;
        public Status status = Status.OutElevator;
        public string Name { get; set; }
        public Base _base;
        Random random = new Random();
        
        public Agent(string name, Base b)
        {
            int n = random.Next(12);
            if (n < 12) this.securityLevel = SecurityLevel.TopSecret;
            if (n < 8) this.securityLevel = SecurityLevel.Secret;
            if (n < 4) this.securityLevel = SecurityLevel.Confidential;
            this._base = b;
            this.Name = name;
            Console.WriteLine($"{this.Name} was created with {this.securityLevel} security level");
        }
        public void EnterBase()
        {
            this.currentFloor = Level.G;
            Thread.Sleep(100);
        }
        public AgentActivity GetRandomActivity()
        {
            int n = random.Next(12);
            if (n < 4) return AgentActivity.Wander;
            if (n < 8) return AgentActivity.CallElevator;
            return AgentActivity.Leave;
        }
        public Level GetRandomLevel(Level currLevel)
        {
            Level chosenLevel;
            do
            { // making sure the agent doesn't call the same floor
                int n = random.Next(12);
                chosenLevel = Level.T2;
                if (n < 9) chosenLevel = Level.T1;
                if (n < 6) chosenLevel = Level.S;
                if (n < 3) chosenLevel = Level.G;     
            } while (chosenLevel == currLevel);
            return chosenLevel;
        }

        public void CallElevator()
        {
            _base.elevator.RegisterCall(currentFloor);
            Console.WriteLine($"{this.Name} called the elevator on {_base.TranslateLevel(this.currentFloor)}");
            this.status = Status.WaitElevator;
            ElevatorEngagement();
        }
        public void ElevatorEngagement()
        {
            while (this.status == Status.WaitElevator) // waiting elevator
            {
                if (_base.elevator.CurrentLevel == this.currentFloor && _base.elevator.Enter(this)) //trying to enter the elevator
                {
                    this.status = Status.InElevator;
                    Level wantedLevel = GetRandomLevel(this.currentFloor);
                    _base.elevator.RegisterCall(wantedLevel, this);
                    Console.WriteLine($"{this.Name} entered the elevator and wants to go to {_base.TranslateLevel(wantedLevel)}.");
                    while (this.status == Status.InElevator) //trying to leave the elevator
                    {
                        if (_base.elevator.CurrentLevel == wantedLevel) // wait for the elevator to reach the floor
                        {
                            if (!_base.elevator.Leave(this)) //check if door will open for agent to leave
                            {
                                wantedLevel = GetRandomLevel(_base.elevator.CurrentLevel); // choose another floor if agent rejected
                                _base.elevator.RegisterCall(wantedLevel, this);
                                Console.WriteLine($"{this.Name} got rejected and now wants to go to {_base.TranslateLevel(wantedLevel)}.");
                                continue;
                            }
                            this.status = Status.OutElevator; //leaving elevator
                            this.currentFloor = wantedLevel;
                            Console.WriteLine($"{this.Name} left the elevator at {_base.TranslateLevel(currentFloor)}.");
                        }
                        Thread.Sleep(500);
                    }
                }
                Thread.Sleep(500);
            }
        }
        public void PaintTheBaseRed()
        {
            EnterBase();
            bool inBase = true;
            while (inBase)
            {
                AgentActivity nextActivity = GetRandomActivity();
                switch (nextActivity)
                {
                    case AgentActivity.Wander:
                        Console.WriteLine($"{this.Name} is wandering the halls of the base' {_base.TranslateLevel(this.currentFloor)}.");
                        Thread.Sleep(2000);
                        break;
                    case AgentActivity.CallElevator:
                        Console.WriteLine($"{this.Name} called the elevator on {_base.TranslateLevel(this.currentFloor)}.");
                        CallElevator();
                        Thread.Sleep(2000);
                        break;
                    case AgentActivity.Leave:
                        Console.WriteLine($"{this.Name} decided to leave the base.");
                        inBase = false;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            Console.WriteLine($"{Name} is going back home.");
        }
    }
   
}
