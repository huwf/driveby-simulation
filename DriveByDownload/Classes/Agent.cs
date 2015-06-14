using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{

    public enum AgentState
    {
        Clean,
        Vulnerable,
        Infected
    };

    public class Agent
    {       
        
        public int UnitsCompromised { get; set; }
        public int RecoveryTime { get; set; }
        public Double AttackSuccessProbability { get; set; }
        public Double RecoveryProbability { get; set; }
        public AgentState State { get; set; }
        public Boolean IsVigilant { get; set; }

        public Agent()
        {

        }

        public Agent(Boolean vigilant)
        {
            this.IsVigilant = vigilant;
            this.RecoveryProbability = vigilant ? this.GetVigilantRecoveryProbability() : this.GetNonVigilantRecoveryProbability();
        }


        public virtual void UpdateInformationForTurn()
        {

        }

        public virtual Boolean Attack(Agent a, Double attackSuccessProbability)
        {
            if (!(a.State == AgentState.Infected))
            {
                Boolean infected = Utility.SetBoolByProbability(attackSuccessProbability);
                
                if (infected)
                {
                    a.State = AgentState.Infected;
                    a.UnitsCompromised = 1;
                    return true;
                }
            }
            return false;
        }

        public virtual double GetVigilantRecoveryProbability()
        {

            return 1;// Globals.Randy.NextDouble();
        }

        public virtual double GetNonVigilantRecoveryProbability()
        {
            return 1;//Globals.Randy.NextDouble();
        }

    }
}
