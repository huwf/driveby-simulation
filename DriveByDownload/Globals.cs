using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DriveByDownload.Classes;
using System.Configuration;

namespace DriveByDownload
{
    public static class Globals
    {
        public static SingleRandom Randy;
        //Random generation dictionaries
        public static Dictionary<double, Browser> BrowserDict;
        public static Dictionary<double, CMS> CMSProbabilityDict;
        public static Dictionary<double, Country> HostCountryDict;

        //String keys dictionaries
        public static Dictionary<String, Country> Countries;

        
        //public static Dictionary<String, CMS> CMSDict;

        //General Lists of Stuff
        public static List<User> Users;
        public static List<User> InfectedUsers;
        public static List<User> DistinctInfectedUsers;

        public static String Condition;
        
        public static List<Website> WebsiteList;
        public static List<Website> InfectedWebsites;
        public static List<Website> DistinctInfectedWebsites;

        public static List<Attacker> Attackers;
        public static List<CMS> CMSList;

        public static List<String> Browsers;
        //Countries
        public static List<Country> ScanningCountries;
        public static String [] EUCountries = {"Austria","Belgium","Bulgaria","Croatia","Cyprus","Czech Republic","Denmark","Estonia","Finland","France","Germany","Greece","Hungary","Ireland","Italy","Latvia","Lithuania",
                                                  "Luxembourg","Malta","Netherlands","Poland","Portugal","Romania","Slovakia","Slovenia","Spain","Sweden","United Kingdom"};

        //public static String[] CybercrimeConventionCountries = {"Albania","Andorra","Armenia","Austria","Azerbaijan","Belgium","Bosniaand Herzegovina","Bulgaria","Croatia","Cyprus","Czech Republic","Denmark","Estonia", "Finland",
        //                                                       "France","Georgia","Germany","Greece","Hungary","Iceland","Italy","Latvia","Liechtenstein","Lithuania","Luxembourg","Malta","Moldova","Monaco","Montenegro","Netherlands, The",
        //                                                       "Norway","Poland","Portugal","Romania","Serbia","Slovakia","Slovenia","Spain","Sweden","Switzerland", "Macedonia,Former Yugoslav Republic of","Turkey","Ukraine","United Kingdom"};


        //Parameters
        public static int TURNS = int.Parse(ConfigurationManager.AppSettings["turns"]);
        public static int SAMPLE_SIZE = int.Parse(ConfigurationManager.AppSettings["sampleSize"]);
        public static int USERS = int.Parse(ConfigurationManager.AppSettings["users"]);
        public static int WEBSITES = int.Parse(ConfigurationManager.AppSettings["websites"]);
        public static int ATTACKERS = int.Parse(ConfigurationManager.AppSettings["attackers"]);
        public static int HOSTS = int.Parse(ConfigurationManager.AppSettings["hosts"]);
        public static int SEED = 1233077111;
        //[ThreadStatic]
        //public static int SIMULATION_SEED;

        public static double COMPLIANT_HOSTS = double.Parse(ConfigurationManager.AppSettings["compliantHosts"]);
        public static double HOST_EFFECTIVENESS = double.Parse(ConfigurationManager.AppSettings["hostEffectiveness"]);
        public static int HOST_NOTIFICATION_DAYS = int.Parse(ConfigurationManager.AppSettings["hostNotificationDays"]);
        
        //CMS Probability of vulnerability, and probability of success
        //More likely to find a vulnerability in a CMS, but assume they are better written
        //so less likely to exploit
        public static double CMS_VULNERABILITY = double.Parse(ConfigurationManager.AppSettings["cmsVulnerability"]);
        public static double NON_CMS_VULNERABILITY = CMS_VULNERABILITY / 10;

        public static double BROWSER_VULNERABILITY = double.Parse(ConfigurationManager.AppSettings["newBrowserVulnerability"]);

        public static double CMS_ATTACK_SUCCESS = double.Parse(ConfigurationManager.AppSettings["cmsAttackSuccess"]);
        public static double NON_CMS_ATTACK_SUCCESS = CMS_ATTACK_SUCCESS * 2;

        //Even if the browser is out of date, attacks are not guaranteed to be successful.  If it's current, the probability is 0
        public static double VULNERABLE_BROWSER_ATTACK_SUCCESS_PROBABILITY = double.Parse(ConfigurationManager.AppSettings["browserAttackSuccess"]);


        //Chance of recovery is the same for transitioning from V -> C, and I -> C
        //public static double WEBSITE_CHANCE_OF_RECOVERY = double.Parse(ConfigurationManager.AppSettings["websiteChanceOfRecovery"]);
        public static int WEBSITE_DAYS_BEFORE_FIX = int.Parse(ConfigurationManager.AppSettings["websiteDaysBeforeVulnFix"]);
        public static double WEBSITE_BLACKLIST_CHANCE_OF_FIX = double.Parse(ConfigurationManager.AppSettings["websiteBlacklistChanceOfFix"]);

        public static double WEBSITE_CHANCE_OF_INFECTION = double.Parse(ConfigurationManager.AppSettings["vulnerableWebsiteChanceOfInfection"]);
        public static double WEBSITE_CHANCE_OF_INFECTION_NON_VULNERABLE;
        
        //public static double USER_CHANCE_OF_RECOVERY = double.Parse(ConfigurationManager.AppSettings["userChanceOfRecovery"]) ;

        //Vigilance
        //What is the probability that a given user or website operator will be vigilant
        public static double VIGILANT_WEBSITE = double.Parse(ConfigurationManager.AppSettings["vigilantWebsite"]);
        public static double VIGILANT_USER = double.Parse(ConfigurationManager.AppSettings["vigilantUser"]);

        //What is the probability that a vigilant or non vigilant user/website will upgrade after discovering a vulnerability?
        public static double VIGILANT_USER_RECOVERY = double.Parse(ConfigurationManager.AppSettings["vigilantUserRecovery"]);
        public static double VIGILANT_WEBSITE_RECOVERY = double.Parse(ConfigurationManager.AppSettings["vigilantWebsiteRecovery"]);

        public static double NON_VIGILANT_USER_RECOVERY = double.Parse(ConfigurationManager.AppSettings["nonVigilantUserRecovery"]);
        public static double NON_VIGILANT_WEBSITE_RECOVERY = double.Parse(ConfigurationManager.AppSettings["nonVigilantWebsiteRecovery"]);        




        //public static int TotalRecoveredWebsites;
        //public static int TotalRecoveredUsers;
        
    }
}
