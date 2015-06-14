using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DriveByDownload.Classes;
using System.Threading;
using System.Configuration;
using System.IO;

namespace DriveByDownload
{
    class Program
    {
        //static String Path = @"C:\Users\Huw\Documents\Visual Studio 2010\Projects\DriveByDownload\DriveByDownload\Results\";
        //Recording
        #region----------------------------------------------------------------------------------------------------Static Variables
        [ThreadStatic] 
        public static int TurnCompromisedVigilantUsers;
        [ThreadStatic] 
        public static int TurnRecoveredVigilantUsers;
        [ThreadStatic] 
        public static int TotalCompromisedVigilantUsers;
        [ThreadStatic] 
        public static int TotalRecoveredVigilantUsers;

        [ThreadStatic]
        public static int TurnCompromisedNonVigilantUsers;
        [ThreadStatic]
        public static int TurnRecoveredNonVigilantUsers;
        [ThreadStatic]
        public static int TotalCompromisedNonVigilantUsers;
        [ThreadStatic]
        public static int TotalRecoveredNonVigilantUsers;


        [ThreadStatic] 
        public static int TurnCompromisedVigilantWebsites;
        [ThreadStatic] 
        public static int TurnRecoveredVigilantWebsites;
        [ThreadStatic]
        public static int TotalCompromisedVigilantWebsites;
        [ThreadStatic]
        public static int TotalRecoveredVigilantWebsites;


        [ThreadStatic]
        public static int TurnCompromisedNonVigilantWebsites;
        [ThreadStatic] 
        public static int TurnRecoveredNonVigilantWebsites;
        [ThreadStatic]
        public static int TotalCompromisedNonVigilantWebsites;
        [ThreadStatic]
        public static int TotalRecoveredNonVigilantWebsites;

        #endregion

        static void Main(string[] args)
        {
            //Stats.Turn150Results("intervention-default-5-conditions.csv", 1, false);
            //Stats.InterventionLevelsANOVA("intevention-levels-anova.csv",1);
            //Note: We get a divide by zero error if there are populations without any members, so, remove references to % for those conditions
            //Stats.VigilanceANOVA("vigilance-anova-extra.csv",0);
            //Stats.FullVigilanceInterventionANOVA("vigilance-intervention-anova.csv", 0);
            //Stats.NewVigilanceInterventionANOVA("simple-vigilance-intervention-anova.csv", 0);
            //Console.ReadLine();
            //return;


            DateTime startTime = DateTime.Now;
            String condition = "test";
            String countries = "EU";

            if (args != null)
            {
                if (args.Length == 2)
                {
                    int websitePopulation;
                    double infectionProbability;
                    if (int.TryParse(args[0], out websitePopulation))
                    {
                        //Then we care about website population
                        Globals.WEBSITES = websitePopulation;
                    }

                    else if (double.TryParse(args[0], out infectionProbability))
                    {
                        Globals.WEBSITE_CHANCE_OF_INFECTION = infectionProbability;
                    }
                    else
                    {
                        countries = args[0];    
                    }
                    
                    condition = args[1];
                }
                else if (args.Length == 3)
                {
                    //Countries with levels of effectiveness
                    countries = args[0];
                    Globals.COMPLIANT_HOSTS = double.Parse(args[1]);
                    condition = args[2];
                }
                else if (args.Length == 4)
                {
                    countries = args[0];
                    Globals.VIGILANT_WEBSITE = double.Parse(args[1]);
                    Globals.VIGILANT_USER = double.Parse(args[2]);
                    condition = args[3];
                }
                else if (args.Length == 5)
                {
                    countries = args[0];
                    Globals.COMPLIANT_HOSTS = double.Parse(args[1]);
                    Globals.HOST_EFFECTIVENESS = double.Parse(args[2]);
                    Globals.HOST_NOTIFICATION_DAYS = int.Parse(args[3]);
                    condition = args[4];
                }
                else if (args.Length == 7)
                {
                }
            }
            Globals.Condition = condition;

            DoEverything(Globals.Condition, countries);
            
            //var threads = new List<Thread>();
            //for (int k = 0; k < 1; k++)
            //{
            //    Thread t = new Thread(DoEverything);
            //    t.Name = k.ToString();
            //    t.Start();
            //    threads.Add(t);
            //    Thread.Sleep(1000);
            //}
            //foreach (var thread in threads)
            //{
            //    thread.Join();
            //}
            DateTime FinishTime = DateTime.Now;

            TimeSpan span = new TimeSpan();
            span = FinishTime - startTime;
            Console.WriteLine("For " + Globals.SAMPLE_SIZE + " simulations with " + Globals.TURNS + " turns.  It took " + span.Hours + ":" + String.Format("{0:00}", span.Minutes) + ":" + String.Format("{0:00}", span.Seconds));

        }
        
        static void HostingProvidersScan()
        {
            foreach (Country c in Globals.ScanningCountries)//Globals.Countries.Keys)
            {
                //Country c = (Country)Globals.Countries[s];
                if (c.Scans)
                {
                    foreach (Host h in c.HostList)
                    {
                        h.TakeTurn();
                    }
                }
            }
        }

        static void CMSVulnerabilityAssignment()
        {
            System.Diagnostics.Debug.WriteLine("Going through the CMS list...");
            foreach (CMS c in Globals.CMSList)
            {
                //Be nice.  Assume that on the turn it recovers, it can't become vulnerable again.
                if (c.IsVulnerable)
                    c.IsVulnerable = false;
                else
                {
                    double vulnerabilityProbability = c.Name == "Other" ? 0 : Globals.CMS_VULNERABILITY; //? Globals.NON_CMS_VULNERABILITY : Globals.CMS_VULNERABILITY;
                    c.IsVulnerable = Utility.SetBoolByProbability(vulnerabilityProbability);
                }
            }
        }

        static void BrowserVulnerabilityAssignment()
        {
            System.Diagnostics.Debug.WriteLine("Going through the browser list...");
            foreach (String s in Globals.Browsers)
            {
                Boolean newVulnerability = Utility.SetBoolByProbability(Globals.BROWSER_VULNERABILITY);
                if (newVulnerability)
                {
                    foreach (User u in Globals.Users)
                    {
                        if (u.UserBrowser.Name == s)
                            u.State = AgentState.Vulnerable;
                    }
                }
            }
        }

        static void UsersVisitWebsites()
        {
            System.Diagnostics.Debug.WriteLine("Going through the Users list");
            foreach (User u in Globals.Users)
            {
                foreach (Website w in u.WebsitesToVisit)
                {
                    if (!w.ActionTaken)//Then no-one can visit it, because it's offline
                        u.VisitWebsite(w);
                }
                u.UpdateInformationForTurn();
            }
        }

        static void WebsiteStuff()
        {
            System.Diagnostics.Debug.WriteLine("Going through the websites list");
            foreach (Website w in Globals.WebsiteList)
            {
                w.UpdateInformationForTurn();
                //if (w.State == AgentState.Infected)
                //{
                //    AmountInfected += 1;
                //}
                //else if (w.State == AgentState.Vulnerable)
                //{
                //    AmountVulnerable += 1;
                //}
            }
        }


        static void DoEverything(String condition, String countries)
        {
            DateTime startTime = DateTime.Now;
            var means = new Dictionary<Double, int>();
            //Run this N times.  Current figure is in app.config
            for (int N = 1; N <= Globals.SAMPLE_SIZE; N++)
            {
                Globals.Randy = new SingleRandom(Globals.SEED);
                Console.WriteLine("Starting simulation " + N);

                int simulationSeed = 0;
                int simulationId = 0;
                var data = new Data();
                int j = 1;
                
                var gp = new GenerateParameters(countries);
                

                while (simulationId == 0)
                {
                    try
                    {
                        simulationSeed = Environment.TickCount;
                        simulationId = data.WriteSimulationParameters(simulationSeed, Globals.COMPLIANT_HOSTS, 1.0,
                                                    Globals.WEBSITE_DAYS_BEFORE_FIX,
                                                    Globals.HOST_NOTIFICATION_DAYS,
                                                    condition);      
                    
                    }
                    catch (SqlException sqle)
                    {
                        Console.WriteLine("This is the panic office.  Section 917 may have been hit.  Activate the following procedure.  Attempt " + j);
                        
                        int sleepy = new Random().Next(16, 250);
                        Console.WriteLine("About to sleep for 0." + sleepy + "seconds");
                        Thread.Sleep(sleepy);
                        j++;
                    }
                }
                if(Globals.ScanningCountries.Count > 0)
                    data.InsertScanningCountries(simulationId, Globals.ScanningCountries);
                //Change the seed from the one to generate the network to the one to run the simulation
                Globals.Randy = new SingleRandom(simulationSeed);
                
                System.Diagnostics.Debug.WriteLine(DateTime.Now);


                //Console.WriteLine("Starting simulation " + N);
                for (int i = 1; i <= Globals.TURNS; i++)
                {
                    System.Diagnostics.Debug.WriteLine("-----------------------------------------------------------------Starting turn " + (i));

                    //Countries who require hosts to scan will do that here
                    HostingProvidersScan();
                    //CMS either recovers from vulnerability, or becomes vulnerable, or no change
                    CMSVulnerabilityAssignment();
                    //Browsers stuff here
                    BrowserVulnerabilityAssignment();
                    //Users stuff here
                    UsersVisitWebsites();
                    //Website stuff here
                    WebsiteStuff();
                    //Save the data for the turn
                    data.SaveSimulationTurnData(simulationId, i, TurnCompromisedVigilantUsers, TurnRecoveredVigilantUsers, TotalCompromisedVigilantUsers, TotalRecoveredVigilantUsers,
                                                TurnCompromisedNonVigilantUsers, TurnRecoveredNonVigilantUsers, TotalCompromisedNonVigilantUsers, TotalRecoveredNonVigilantUsers,
                                                TurnCompromisedVigilantWebsites, TurnRecoveredVigilantWebsites, TotalCompromisedVigilantWebsites, TotalRecoveredVigilantWebsites,
                                                TurnCompromisedNonVigilantWebsites, TurnRecoveredNonVigilantWebsites, TotalCompromisedNonVigilantWebsites, TotalRecoveredNonVigilantWebsites); 

                    //Clear them all for the end of the turn
                    TurnCompromisedVigilantUsers = 0;
                    TurnRecoveredVigilantUsers = 0;

                    TurnCompromisedNonVigilantUsers = 0;
                    TurnRecoveredNonVigilantUsers = 0;

                    TurnCompromisedVigilantWebsites = 0;
                    TurnRecoveredVigilantWebsites = 0;


                    TurnCompromisedNonVigilantWebsites = 0;
                    TurnRecoveredNonVigilantWebsites = 0;


                }//Each turn
                
                //Clear the totals after the simulation
                TotalCompromisedVigilantUsers = 0;
                TotalRecoveredVigilantUsers = 0;
                
                TotalCompromisedNonVigilantUsers = 0;
                TotalRecoveredNonVigilantUsers = 0;


                TotalCompromisedVigilantWebsites = 0;
                TotalRecoveredVigilantWebsites = 0;
                

                TotalCompromisedNonVigilantWebsites = 0;
                TotalRecoveredNonVigilantWebsites = 0;

                Console.WriteLine("Finishing");
            }//Each simulation

            //Outside of the N loop            
            DateTime FinishTime = DateTime.Now;
            TimeSpan span = new TimeSpan();
            span = FinishTime - startTime;
            Console.WriteLine("For " + Globals.SAMPLE_SIZE + " simulations with " + Globals.TURNS + " turns.  It took " + span.Hours + ":" + String.Format("{0:00}", span.Minutes) + ":" + String.Format("{0:00}", span.Seconds));
            //Console.ReadLine();
        }




        /// <summary>
        /// Not really used anymore
        /// </summary>
        /// <param name="filename"></param>
        //static void HeatMapCSV(String filename)
        //{
        //    String path = ConfigurationManager.AppSettings["resultsPath"].ToString() + "\\" + Program.Condition + "\\";

        //    //Loop for vigilant websites
        //    for (double i = 1; i > 0; i -= 0.1)
        //    {
        //        //loop for vigilant users
        //        for (double j = 0.1; j <= 1; j += 0.1)
        //        {
        //            String test = "Wv" + i.ToString() + "-Uv" + j.ToString();
        //            double average = GetTurnAverage(path + "\\CombinedResults\\" + test, filename, Globals.TURNS);
        //            Console.WriteLine("Average: " + average);
        //            File.AppendAllText(path + "\\heatmap-" + filename, average + ",");

        //        }
        //        File.AppendAllText(path + "\\heatmap-" + filename, "\n");
        //    }
        //}

        /// <summary>
        /// Not used atm, will probably just use the dtabase.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        static double GetTurnAverage(String path, String filename, int turn)
        {

            String csvText = File.ReadAllText(path + "\\" + filename);

            List<int> turnValues = new List<int>();
            String[] lines = csvText.Split('\n');
            foreach (String csv in lines)
            {
                if (!String.IsNullOrEmpty(csv))
                {
                    String value = csv.Split(',')[turn - 1];
                    turnValues.Add(int.Parse(value));
                    Console.WriteLine(value);
                }

            }

            return (double)turnValues.Sum() / turnValues.Count;
        }


    }
}
