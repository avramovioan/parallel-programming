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
        public Level currentFloor = Level.G;
        public Status status = Status.OutElevator;
        public string Name { get; set; }
        public Base _base;
        Random random = new Random();
        
        public Agent(string name, Base b)
        {
            int n = random.Next(12);
            if (n < 4) this.securityLevel = SecurityLevel.Confidential;
            if (n < 8) this.securityLevel = SecurityLevel.Secret;
            if (n < 12) this.securityLevel = SecurityLevel.TopSecret;
            this._base = b;
        }
        public void EnterBase()
        {
            Console.WriteLine($"{this.Name} entered the base.");
            this.currentFloor = Level.G;
            Thread.Sleep(100);
        }
        public AgentActivity GetRandomActivity()
        {
            int n = random.Next(12);
            if (n < 5) return AgentActivity.Wander;
            if (n < 10) return AgentActivity.CallElevator;
            return AgentActivity.Leave;
        }
        public Level GetRandomLevel()
        {
            Level chosenLevel;
            int n = random.Next(12);
            do
            {
                if (n < 3) chosenLevel = Level.G;
                if (n < 6) chosenLevel = Level.S;
                if (n < 9) chosenLevel = Level.T1;
                chosenLevel = Level.T2;
            } while (chosenLevel != currentFloor);
            return chosenLevel;
        }

        public void CallElevator()
        {
            _base.elevator.RegisterExternalCall(currentFloor);
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
                    Level wantedLevel = GetRandomLevel();
                    _base.elevator.RegisterInternalCall(wantedLevel, this);
                    Console.WriteLine($"{this.Name} entered the elevator and wants to go to {TranslateLevel(wantedLevel)}.");
                    while (this.status == Status.InElevator) //trying to leave the elevator
                    {
                        if (_base.elevator.CurrentLevel == this.currentFloor) 
                        {
                            if (!_base.elevator.Leave(this)) // choose another floor if agent rejected
                            {
                                wantedLevel = GetRandomLevel();
                                _base.elevator.RegisterInternalCall(wantedLevel, this);
                                Console.WriteLine($"{this.Name} got rejected and now wants to go to {TranslateLevel(wantedLevel)}.");
                                continue;
                            }
                            this.status = Status.OutElevator; //leaving elevator
                            this.currentFloor = wantedLevel;
                            Console.WriteLine($"{this.Name} left the elevator at {TranslateLevel(currentFloor)}.");
                        }
                        Thread.Sleep(500);
                    }
                }
                Thread.Sleep(1000);
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
                        Console.WriteLine($"{this.Name} is wandering the halls of the base' {TranslateLevel(this.currentFloor)}.");
                        break;
                    case AgentActivity.CallElevator:
                        Console.WriteLine($"{this.Name} called the elevator on {TranslateLevel(this.currentFloor)}.");
                        CallElevator();
                        break;
                    case AgentActivity.Leave:
                        Console.WriteLine($"{this.Name} decided to leave the base and go home.");
                        inBase = false;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            Console.WriteLine($"{Name} is going back home.");
        }
        public string TranslateLevel(Level level)
        {
            switch (level)
            {
                case Level.G:
                    return "Ground floor";
                case Level.S:
                    return "Secret floor";
                case Level.T1:
                    return "Top secret floor";
                case Level.T2:
                    return "Ultra-Top secret floor";
                default:
                    throw new NotImplementedException();
            }
        }
    }
   
}
