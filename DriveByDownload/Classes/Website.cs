using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DriveByDownload.Classes
{


    public class Website : Agent
    {
        //TODO: Need to get the website to act based on whether they have been notified
        public Website(String url, int visitsPerMillion, int views, Boolean vigilant) : base(vigilant)
        {   
            this.Url = url;
            this.VisitsPerMillion = visitsPerMillion;
            this.ViewsPerUser = views;
            this.SetCMSSoftware();
            this.ActionTaken = false;
        }
        
        public String Url { get; set; }
        public int VisitsPerMillion { get; set; }
        public int ViewsPerUser { get; set; }

        private CMS _CMSSoftware;
        public CMS CMSSoftware { get{return this._CMSSoftware;} set{this._CMSSoftware = value;} }        
        
        public Host HostingProvider { get; set; }
        public Boolean IsBlacklisted { get; set; }
        public int DaysNotified { get; set; }
        public Boolean ActionTaken { get; set; }

        public void SetCMSSoftware()
        {
            Double randy = Globals.Randy.NextDouble();
            double index = Utility.GenerateObjectIndex(randy, Globals.CMSProbabilityDict.Keys.ToList());            
            CMS c = Globals.CMSProbabilityDict[index];
            this.CMSSoftware = c;
            c.Websites.Add(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="successProbability"></param>
        /// <returns></returns>
        public override Boolean Attack(Agent a, Double successProbability) 
        {
            User u = (User)a;
            
            Boolean success = base.Attack(u,successProbability);
            if (success)
            {
                System.Diagnostics.Debug.WriteLine("Thread" + System.Threading.Thread.CurrentThread.Name + " ****USER INFECTION!!!****" + u.GetHashCode());
                if (u.IsVigilant)
                {
                    Program.TurnCompromisedVigilantUsers += 1;
                    Program.TotalCompromisedVigilantUsers += 1;
                }
                else
                {
                    Program.TurnCompromisedNonVigilantUsers += 1;
                    Program.TotalCompromisedNonVigilantUsers += 1;
                }
                
                //Program.TotalCompromisedUsers += 1;
                //if(!Globals.InfectedUsers.Contains(u))
                //    Globals.InfectedUsers.Add(u);
            }
            return success;
        }

        /// <summary>
        /// Simulates a user visiting a webpage.  If they are infected, the page will attack.  If not, nothing happens
        /// </summary>
        /// <param name="u">The user to display the page to, or to attack</param>
        public void DisplayPage(User u)
        {            
            Double successProbability = u.UserBrowser.Version == "Current" ? 0 : Globals.VULNERABLE_BROWSER_ATTACK_SUCCESS_PROBABILITY;

            if (this.State == AgentState.Infected)
                this.Attack(u, successProbability);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void UpdateInformationForTurn()
        {
            //Moved blocking recovery to Host.cs.  Because it makes more sense...
            if (this.State == AgentState.Clean)
            {
                this.VulnerableTransition();

            }
            else if (this.State == AgentState.Vulnerable)
            {                
                //Vulnerable to clean
                if (this.UnitsCompromised < Globals.WEBSITE_DAYS_BEFORE_FIX)
                {
                    this.UnitsCompromised += 1;
                }
                else
                {
                    this.RecoveryTransition(this.RecoveryProbability);
                }
                    
            }
            else if (this.State == AgentState.Infected)
            {
                //If they're infected, they can't get infected again on the same turn even if they recover.
                this.RecoveryTransition(this.RecoveryProbability);
                return;
            }
            //See if the website will get infected this turn
            this.InfectionTransition();       
        }


        #region---------------------------------------------------------------------------------------------------------Transitions
        private void VulnerableTransition()
        {            
            if (this.CMSSoftware.IsVulnerable)
            {
                this.State = AgentState.Vulnerable;
            }
            else if (this.CMSSoftware.Name == "No CMS")
            {
                //If it's not a CMS, the CMSSoftware.IsVulnerable will always be false
                //This allows the possibility of infection without a CMS.
                if (Utility.SetBoolByProbability(Globals.NON_CMS_VULNERABILITY))
                {
                    this.State = AgentState.Vulnerable;
                }
            }            
        }

        public bool InfectionTransition()
        {
            double probability = this.State == AgentState.Vulnerable ? Globals.WEBSITE_CHANCE_OF_INFECTION : Globals.WEBSITE_CHANCE_OF_INFECTION_NON_VULNERABLE;
            bool infect = Utility.SetBoolByProbability(probability);
            if (infect)
            {
                this.State = AgentState.Infected;
                if (this.IsVigilant)
                {
                    Program.TurnCompromisedVigilantWebsites += 1;
                    Program.TotalCompromisedVigilantWebsites += 1;
                }
                else
                {
                    Program.TurnCompromisedNonVigilantWebsites += 1;
                    Program.TotalCompromisedNonVigilantWebsites += 1;
                }
            }
            return infect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recoveryProbability"></param>
        public bool RecoveryTransition(Double recoveryProbability)
        {
            bool recover = Utility.SetBoolByProbability(recoveryProbability);
            if (recover)
            {
                //Don't need to record anything if it recovers from vulnerable
                if (this.State == AgentState.Infected)
                {
                    if (this.IsVigilant)
                    {
                        Program.TurnRecoveredVigilantWebsites += 1;
                        Program.TotalRecoveredVigilantWebsites += 1;
                    }
                    else
                    {
                        Program.TurnRecoveredNonVigilantWebsites += 1;
                        Program.TotalRecoveredNonVigilantWebsites += 1;
                    }                    
                }
                
                this.State = AgentState.Clean;
                this.CMSSoftware.Version = "Current";

            }
            return recover;
        }

        #endregion


        public override Double GetVigilantRecoveryProbability(){
            return Globals.VIGILANT_WEBSITE_RECOVERY;
        }

        public override double GetNonVigilantRecoveryProbability()
        {
            return Globals.NON_VIGILANT_WEBSITE_RECOVERY;
        }


    }
}
