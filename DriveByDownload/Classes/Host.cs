using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    public class Host
    {
        public Host(String name, Country country, Boolean compliant)
        {
            this.Name = name;
            this.Country = country;
            this.Compliant = compliant;
            this.Websites = new List<Website>();
        }

        public String Name { get; set; }
        public Country Country { get; set; }
        //public Boolean ScansSites { get; set; }
        public List<Website> Websites { get; set; }
        public Boolean Compliant { get; set; }

        public void TakeTurn()
        {
            //If they're not compliant, they just don't do anything!
            if (this.Compliant)
            {
                foreach (Website w in this.Websites)
                {
                    if (w.ActionTaken)
                    {
                        if (w.State == AgentState.Clean)
                        {
                            w.DaysNotified = 0;
                            w.ActionTaken = false;
                        }
                        
                    }
                    else
                    {
                        //If the website is vulnerable, either give them a warning, or take action
                        if (w.State == AgentState.Vulnerable || w.State == AgentState.Infected)
                        {
                            //Take action with certain probability:
                            //If they have already notified, then that effectiveness has already occurred
                            if (Utility.SetBoolByProbability(Globals.HOST_EFFECTIVENESS) || w.DaysNotified > 0)
                            {
                                if (w.DaysNotified < Globals.HOST_NOTIFICATION_DAYS)
                                    w.DaysNotified += 1;
                                //Added in this extra if statement.  Hasn't been run yet.
                                else if (w.DaysNotified >= Globals.HOST_NOTIFICATION_DAYS || w.State == AgentState.Infected)
                                {
                                    w.ActionTaken = true;
                                }
                            }
                        }                        
                    }

                }
            }
        }


    }
}
