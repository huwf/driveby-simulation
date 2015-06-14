using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    public class User : Agent
    {
        public Browser UserBrowser { get; set; }
        public int DiligenceLevel { get; set; }
        public List<Website> WebsitesToVisit { get; set; }



        public User(Browser userBrowser, int recoveryTime, Boolean vigilant)
            : base(vigilant)
        {
            this.UserBrowser = userBrowser;
            //this.State = vigilant ? AgentState.Infected : AgentState.Clean;
            this.RecoveryTime = recoveryTime;
            this.UnitsCompromised = 0;
            this.RecoveryProbability = this.IsVigilant
                ? Globals.VIGILANT_USER_RECOVERY
                : Globals.NON_VIGILANT_USER_RECOVERY;
            //if (this.IsVigilant)
            //{
            //    System.Diagnostics.Debug.WriteLine("Vigilant");
            //}

        }        

        public override void UpdateInformationForTurn()
        {            
            if (this.State == AgentState.Infected)
            {
                bool recover = Utility.SetBoolByProbability(this.RecoveryProbability);

                if (recover)
                {
                    this.State = AgentState.Clean;
                    System.Diagnostics.Debug.WriteLine("Thread" + System.Threading.Thread.CurrentThread.Name + " ****USER RECOVERY!!!****" + this.GetHashCode());
                    if (this.IsVigilant)
                    {
                        Program.TurnRecoveredVigilantUsers += 1;
                        Program.TotalRecoveredVigilantUsers += 1;
                    }
                    else
                    {
                        Program.TurnRecoveredNonVigilantUsers += 1;
                        Program.TotalRecoveredNonVigilantUsers += 1;
                    }
                    
                }
                else
                {
                    this.UnitsCompromised += 1;
                }
            }
            else if (this.State == AgentState.Vulnerable)
            {
                this.UpdateBrowser();
            }
        }

        private void UpdateBrowser()
        {
            Boolean update = Utility.SetBoolByProbability(this.RecoveryProbability);
            if (update)
                this.UserBrowser = new Browser(this.UserBrowser.Name, "Current");
        }

        /// <summary>
        /// Simulates the user visiting a website
        /// </summary>
        /// <param name="w">The website they are visiting</param>
        /// <returns>
        /// true if the user gets infected
        /// false if they don't
        /// </returns>
        public Boolean VisitWebsite(Website w)
        {            
            //If website is compromised, then it will attack
            //Use the browser's probability of compromise to decide whether the attack succeeds
            w.DisplayPage(this);
            return this.State == AgentState.Infected;
        }

        public override Double GetVigilantRecoveryProbability()
        {
            return Globals.VIGILANT_USER_RECOVERY;
        }

        public override double GetNonVigilantRecoveryProbability()
        {
            return Globals.NON_VIGILANT_USER_RECOVERY;
        }


    }
}
