using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DriveByDownload.Classes;
using System.Data.SqlClient;
using System.Data;

namespace DriveByDownload
{
    class GenerateParameters
    {
        private SQL sql;
        
        public int vigilantWebsites = 0;
        public int nonVigilantWebsites = 0;

        public int vigilantUsers = 0;
        public int nonVigilantUsers = 0;

        public GenerateParameters(String countries)
        {
            sql = new SQL();
            //Globals.SIMULATION_SEED = seed;
            
            this.SetWebsiteProbabilityOfInfection();
            Globals.BrowserDict = this.SetBrowserDict();
            Globals.Browsers = this.GetBrowserList();
            Globals.Countries = CountryList();
            Globals.HostCountryDict = SetHostCountryDict();
            
            this.SetCMSSoftwareDict();
            this.CreateCMSs();
            Globals.WebsiteList = this.CreateWebsites();
            Globals.Users = this.CreateUsers();

            //Globals.Attackers = this.CreateAttackers();
            List<String> scanningList = countries.Split('|').ToList();
            Globals.ScanningCountries = this.ScanningCountries(scanningList);
            Data d = new Data();
            d.WriteSimulationPopulations(Globals.SEED, this.vigilantUsers, this.nonVigilantUsers, this.vigilantWebsites, this.nonVigilantWebsites, Globals.Condition);
            
        }

        public List<Website> UserWebsiteVisitList()
        {
            List<Website> websites = new List<Website>();
            foreach (Website w in Globals.WebsiteList)
            {
                Double probability = (Double)w.VisitsPerMillion / 1000000;
                bool visit = Utility.SetBoolByProbability(probability);
                if (visit)
                {
                    websites.Add(w);
                }
            }
            return websites;
        }

        public List<Country> ScanningCountries(List<String> countries)
        {
            if (countries[0] == "EU")
            {
                countries = Globals.EUCountries.ToList();
            }
            else if (countries[0].ToUpper() == "World".ToUpper())
            {
                countries = Globals.Countries.Keys.ToList();
            }
            List<Country> ScanningCountries = new List<Country>();
            foreach (String s in countries)
            {
                if (String.IsNullOrEmpty(s))
                    break;
                Country c = Globals.Countries[s];
                c.Scans = true;
                ScanningCountries.Add(c);
            }
            return ScanningCountries;
        }

        public void SetWebsiteProbabilityOfInfection()
        {
            
            const int infectedPerTurn = 30000;
            const double websites = 182698818;
            //Double Websites = 975262468;
            //Globals.WEBSITE_CHANCE_OF_INFECTION = ;
            Globals.WEBSITE_CHANCE_OF_INFECTION_NON_VULNERABLE = (infectedPerTurn / websites);
        }

        public Dictionary<Double, Browser> SetBrowserDict()
        {

            Dictionary<Double,Browser> Browsers = new Dictionary<Double,Browser>();
            //DataTable dt = this.sql.SelectQuery("SELECT BrowserName, BrowserVersion,StartingNumber FROM BrowserDistribution");            
            DataTable dt = this.sql.SelectQuery("SELECT BrowserName, StartingNumber FROM NewBrowserDistribution");
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                foreach(DataRow row in dt.Rows)
                {

                    String Name = (String)row[0];
                    //String Version = (String)row[1];
                    //Assume that the amount of people with the up to date browser is 85%.  Look at 
                    //http://stats.wikimedia.org/archive/squid_reports/2014-09/SquidReportClients.htm
                    //Top two versions of FF and Chrome are 85/86%.  Seems like a good estimate...
                    //The distribution also comes from there.  Chrome might be slightly high but will do for now.
                    const double upToDateVersionProbability = 0.85;
                    String version = Utility.SetBoolByProbability(upToDateVersionProbability) ? "Current" : "Old";
                    Double startingNumber = (Double)row[1];
                    if (i != 0)
                    {
                        Browsers.Add(startingNumber, new Browser(Name, version));
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            
            return Browsers;
        }

        public List<String> GetBrowserList()
        {
            List<String> browsers = new List<String>();
            DataTable dt = sql.SelectQuery("SELECT DISTINCT BrowserName FROM BrowserDistribution");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    String browserName = (String)row["BrowserName"];
                    browsers.Add(browserName);
                }
            }
            return browsers;
        }

        public Browser GenerateBrowser(Double rnd)
        {
            double key = Utility.GenerateObjectIndex(rnd, Globals.BrowserDict.Keys.ToList());
            Browser b = Globals.BrowserDict[key];
            return new Browser(b.Name, b.Version);
        }

        private Dictionary<String,Country> CountryList()
        {
            Dictionary<string,Country> Countries = new Dictionary<String,Country>();
            DataTable dt = sql.SelectQuery("SELECT CountryId, Country FROM Countries");
            if (dt.Rows.Count > 0)
            {                
                foreach(DataRow row in dt.Rows)
                {
                    int CountryId = (int)row[0];
                    String Name = (String)row[1];
                    Country c = new Country(Name, CountryId);
                    c.Id = CountryId;
                    Countries.Add(Name, c);
                }
            }            
            return Countries;
        }

        /// <summary>
        /// Dictionary with the distribution for Web hosts coming from different countries
        /// </summary>
        /// <returns></returns>
        private Dictionary<Double,Country> SetHostCountryDict()
        {
            Dictionary<Double, Country> Countries = new Dictionary<Double, Country>();
            String Query = "SELECT SUM(Amount) FROM HostPerCountry";
            int TotalDomains = (int)sql.SingleValueQuery(Query);
            Query = "SELECT Country, Amount FROM Countries c INNER JOIN HostPerCountry h ON c.CountryId = h.CountryId WHERE Amount != 0";
            Double RunningTotal = 0.0;
            DataTable dt = sql.SelectQuery(Query);
            if (dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    String CountryString = (String)row[0];
                    Double Probability = (Double)(int)row[1] / TotalDomains;
                    RunningTotal += Probability;
                    Countries.Add(RunningTotal, Globals.Countries[CountryString]);
                }
            }
            return Countries;
        }


        //public List<Attacker> CreateAttackers()
        //{
        //    List<Attacker> attackers = new List<Attacker>();
        //    for (int i = 0; i < Globals.ATTACKERS; i++)
        //    {                
        //        Attacker a = new Attacker();
        //        attackers.Add(a);
        //    }
        //    return attackers;
        //}

        public List<User> CreateUsers()
        {
            Random r = Globals.Randy;

            List<User> users = new List<User>(Globals.USERS);
            for (int i = 0; i < Globals.USERS; i++)
            {
                Double randomNumber = r.NextDouble();
                bool vigilant = Utility.SetBoolByProbability(Globals.VIGILANT_USER);
                User u = new User(this.GenerateBrowser(randomNumber), 4, vigilant);
                
                //u.IsVigilant = vigilant;
                if (vigilant)
                    vigilantUsers += 1;
                else
                    nonVigilantUsers += 1;
                u.WebsitesToVisit = this.UserWebsiteVisitList();
                users.Add(u);
            }
            return users;
        }

        public List<Website> CreateWebsites()
        {

            List<Website> Websites = new List<Website>();
            //Final change, added in an order by for ReachPerMillion.  Some of them were 0, and accounted for by presumably the amount of visits.  We're not measuring
            //this, so need to make sure we get the top Globals.WEBSITES ReachPerMillion values
            DataTable dt = sql.SelectQuery("SELECT TOP " + Globals.WEBSITES + " URL, Country,ReachPerMillion,PageViewsPerUser FROM Websites ORDER BY ReachPerMillion DESC");
            if (dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {

                    String Url = (String)row[0];
                    String Country = (String)row[1];
                    int reachPerMillion = (int)row[2];
                    int views = (int)row[3];
                    Boolean vigilant = Utility.SetBoolByProbability(Globals.VIGILANT_WEBSITE);
                    Website w = new Website(Url, reachPerMillion, views, vigilant);
                    if (vigilant) 
                        vigilantWebsites += 1;
                    else
                        nonVigilantWebsites += 1;
                    Double randyCountry = Utility.GenerateObjectIndex(Globals.Randy.NextDouble(), Globals.HostCountryDict.Keys.ToList());
                    List<Double> hostProbabilities = Globals.HostCountryDict[randyCountry].Hosts.Keys.ToList();
                    Double randyHost = Utility.GenerateObjectIndex(Globals.Randy.NextDouble(), hostProbabilities);
                    w.HostingProvider = Globals.HostCountryDict[randyCountry].Hosts[randyHost];                    
                    //w.IsVigilant = Utility.SetBoolByProbability()
                    Websites.Add(w);

                    //Just for debugging purposes, can take out later if we need. Want to check the distribution seems reasonable
                    w.HostingProvider.Websites.Add(w);
                }
            }
            return Websites;
        }

        /// <summary>
        /// Assuming we will at some stage want both a way of accessing a CMS by name, and a means of assigning probabilities,
        /// this method does both!
        /// </summary>
        public void SetCMSSoftwareDict()
        {
            var CMSProbabilityDict = new Dictionary<Double, CMS>();
            

            DataTable dt = sql.SelectQuery(@"select Name, '1' as Version, 0.369 * Share / 100 as MarketShare from CMSSoftware union select 'No CMS', 'No Version',63.1/100");
            CMS c;
            if (dt.Rows.Count > 0)
            {
                Double totalShare = 0.0;
                foreach (DataRow row in dt.Rows)
                {
                    String CMSName = (String)row["Name"];
                    //On 1/11/14 the market share for the latest version of WP is 35.7%, and 9.7% + 16.5% = 26.2% for Drupal (7.32 and 6.33)
                    //The average of those two figures is 30.95, now saved in app.config
                    //double currentProbability = 0.395;
                    String version = Utility.SetBoolByProbability(Globals.VIGILANT_WEBSITE) ? "Current" : "Old";
                    Double marketShare = (Double)row["MarketShare"];
                    c = new CMS(CMSName, version);
                    totalShare += marketShare;
                    CMSProbabilityDict.Add(totalShare, c);                    
                }
                c = new CMS("Other", "Other");
                CMSProbabilityDict.Add(1.0, c);
            }
            Globals.CMSProbabilityDict = CMSProbabilityDict;            
        }

        public void CreateCMSs()
        {
            Globals.CMSList = Globals.CMSProbabilityDict.Values.ToList();
        }
    }
}
