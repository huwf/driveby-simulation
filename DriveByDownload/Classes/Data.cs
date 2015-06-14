using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    public class Data
    {
        private SQL sql;

        public Data()
        {
            sql = new SQL();
        }


        public int WriteSimulationParameters(int simulationSeed, double compliance, double effectiveness, int daysBeforeFix, int blockingDays, String condition = "")
        {
            var sqlParams = new List<SqlParameter> 
            {
                new SqlParameter("NetworkSeed", Globals.SEED), 
                new SqlParameter("SimulationSeed", simulationSeed),
                new SqlParameter("Websites", Globals.WEBSITES),
                new SqlParameter("Hosts", Globals.HOSTS),
                new SqlParameter("Users", Globals.USERS),
                new SqlParameter("Turns", Globals.TURNS),
                new SqlParameter("VigilantWebsite", Globals.VIGILANT_WEBSITE),
                new SqlParameter("VigilantUser", Globals.VIGILANT_USER),
                new SqlParameter("VulnWebsiteChanceOfInfection", Globals.WEBSITE_CHANCE_OF_INFECTION),
                new SqlParameter("VigilantUserRecoveryInfection", Globals.VIGILANT_USER_RECOVERY),
                new SqlParameter("VigilantWebsiteRecoveryInfection", Globals.VIGILANT_WEBSITE_RECOVERY),
                new SqlParameter("NonVigilantUserRecoveryInfection", Globals.NON_VIGILANT_USER_RECOVERY),
                new SqlParameter("NonVigilantWebsiteRecoveryInfection", Globals.NON_VIGILANT_WEBSITE_RECOVERY),
                new SqlParameter("CMSVulnerability", Globals.CMS_VULNERABILITY),
                new SqlParameter("CMSAttackSuccess", Globals.NON_CMS_ATTACK_SUCCESS),
                new SqlParameter("NewBrowserVulnerability", Globals.BROWSER_VULNERABILITY),
                new SqlParameter("WebsiteBlacklistChanceOfFix", Globals.WEBSITE_BLACKLIST_CHANCE_OF_FIX),
                new SqlParameter("WebsiteDaysBeforeVulnFix", Globals.WEBSITE_DAYS_BEFORE_FIX),
                new SqlParameter("HostCompliance", compliance),
                new SqlParameter("HostEffectiveness",effectiveness),
                new SqlParameter("Condition", condition)
            };

            String query = 
                @"INSERT INTO [Alexa].[dbo].[Simulation]
                (
                    [NetworkSeed],
                    [SimulationSeed],
                    [Websites],
                    [Hosts],
                    [Users],
                    [Turns],
                    [VigilantWebsite],
                    [VigilantUser],
                    [VulnWebsiteChanceOfInfection],
                    [VigilantUserRecoveryInfection],
                    [VigilantWebsiteRecoveryInfection],
                    [NonVigilantUserRecoveryInfection],
                    [NonVigilantWebsiteRecoveryInfection],
                    [CMSVulnerability],
                    [CMSAttackSuccess],
                    [NewBrowserVulnerability],
                    [WebsiteBlacklistChanceOfFix],
                    [WebsiteDaysBeforeVulnFix],
                    [HostCompliance],
                    [HostEffectiveness],
                    [Condition]
                )
                OUTPUT inserted.SimulationId
                VALUES
                (
                    @NetworkSeed,
                    @SimulationSeed,
                    @Websites,
                    @Hosts,
                    @Users,
                    @Turns,
                    @VigilantWebsite,
                    @VigilantUser,
                    @VulnWebsiteChanceOfInfection,
                    @VigilantUserRecoveryInfection,
                    @VigilantWebsiteRecoveryInfection,
                    @NonVigilantUserRecoveryInfection,
                    @NonVigilantWebsiteRecoveryInfection,
                    @CMSVulnerability,
                    @CMSAttackSuccess,
                    @NewBrowserVulnerability,
                    @WebsiteBlacklistChanceOfFix,
                    @WebsiteDaysBeforeVulnFix,
                    @HostCompliance,
                    @HostEffectiveness,
                    @Condition
                )";
            return sql.InsertQuery(query,sqlParams);
        }

        public void InsertScanningCountries(int simulationId, List<Country> countries)
        {
            String query = @"INSERT INTO SimulationCountries(SimulationId,CountryId) OUTPUT inserted.CountryId VALUES(@SimulationId,@CountryId)";
            foreach (var c in countries)
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>
                {
                    new SqlParameter("SimulationId",simulationId),
                    new SqlParameter("CountryId",c.Id)    
                };
                sql.InsertQuery(query, sqlParams);
            }

        }

        public int WriteSimulationPopulations(int seed, int vUsers, int nvUsers, int vWebsites, int nvWebsites, String condition)
        {
            List<SqlParameter> queryParams = new List<SqlParameter> 
            {
                new SqlParameter("NetworkSeed",seed),
                new SqlParameter("VigilantUsers",vUsers),
                new SqlParameter("NonVigilantUsers",nvUsers),
                new SqlParameter("VigilantWebsites",vWebsites),
                new SqlParameter("NonVigilantWebsites",nvWebsites),
                new SqlParameter("Condition", condition)
            };

            //Check to see if this is already in the 
            String testQuery = "SELECT Count(*) FROM NetworkInfo WHERE Condition = @Condition";
            int testCount = (int)sql.SingleValueQuery(testQuery,
                new List<SqlParameter> {new SqlParameter("Condition", condition)});
            if (testCount == 0)
            {
                try
                {
                    String query =
                    @"INSERT INTO [dbo].[NetworkInfo]
                    (
                        [NetworkSeed],
                        [VigilantUsers],
                        [NonVigilantUsers],
                        [VigilantWebsites],
                        [NonVigilantWebsites],
                        [Condition]
                    )
                    OUTPUT inserted.NetworkInfoId
                    VALUES
                    (
                        @NetworkSeed,
                        @VigilantUsers,
                        @NonVigilantUsers,
                        @VigilantWebsites,
                        @NonVigilantWebsites,
                        @Condition
                    )";

                    return sql.InsertQuery(query, queryParams);
                }
                catch (SqlException)
                {
                    Console.WriteLine("Fitter, happier, more productive.   Comfortable, not drinking too much.  Only one NetworkInfo per condition thank you very much :)");
                    return 0;
                }

            }
            else return 0;

        }

        public int SaveSimulationTurnData(
            int simulationId, int turnNo, int infectedVigilantUsers, int recoveredVigilantUsers, int totalInfectedVigilantUsers, int totalRecoveredVigilantUsers,
            int infectedNonVigilantUsers, int recoveredNonVigilantUsers, int totalInfectedNonVigilantUsers, int totalRecoveredNonVigilantUsers,
            int infectedVigilantWebsites, int recoveredVigilantWebsites, int totalInfectedVigilantWebsites, int totalRecoveredVigilantWebsites,
            int infectedNonVigilantWebsites, int recoveredNonVigilantWebsites, int totalInfectedNonVigilantWebsites, int totalRecoveredNonVigilantWebsites
            )
        {
            var queryParams = new List<SqlParameter>
            {
                new SqlParameter("SimulationId" ,simulationId),
                new SqlParameter("TurnNo",turnNo),

                new SqlParameter("VigilantUsersInfected",infectedVigilantUsers),
                new SqlParameter("VigilantUsersRecovered",recoveredVigilantUsers),
                new SqlParameter("VigilantUsersInfectedTotal",totalInfectedVigilantUsers),
                new SqlParameter("VigilantUsersRecoveredTotal",totalRecoveredVigilantUsers),
                
                new SqlParameter("NonVigilantUsersInfected",infectedNonVigilantUsers),
                new SqlParameter("NonVigilantUsersRecovered",recoveredNonVigilantUsers),
                new SqlParameter("NonVigilantUsersInfectedTotal",totalInfectedNonVigilantUsers),
                new SqlParameter("NonVigilantUsersRecoveredTotal",totalRecoveredNonVigilantUsers),


                new SqlParameter("VigilantWebsitesInfected",infectedVigilantWebsites),
                new SqlParameter("VigilantWebsitesRecovered",recoveredVigilantWebsites),
                new SqlParameter("VigilantWebsitesInfectedTotal",totalInfectedVigilantWebsites),
                new SqlParameter("VigilantWebsitesRecoveredTotal",totalRecoveredVigilantWebsites),
                
                new SqlParameter("NonVigilantWebsitesInfected",infectedNonVigilantWebsites),
                new SqlParameter("NonVigilantWebsitesRecovered",recoveredNonVigilantWebsites),
                new SqlParameter("NonVigilantWebsitesInfectedTotal",totalInfectedNonVigilantWebsites),
                new SqlParameter("NonVigilantWebsitesRecoveredTotal",totalRecoveredNonVigilantWebsites)
            };
            String query =
                @"INSERT INTO [dbo].[SimulationTurn]
                (
                    [SimulationId],
                    [TurnNo],
                    [VigilantUsersInfected],
                    [VigilantUsersRecovered],
                    [VigilantUsersInfectedTotal],
                    [VigilantUsersRecoveredTotal],
                    [NonVigilantUsersInfected],
                    [NonVigilantUsersRecovered],
                    [NonVigilantUsersInfectedTotal],
                    [NonVigilantUsersRecoveredTotal],
                    [VigilantWebsitesInfected],
                    [VigilantWebsitesRecovered],
                    [VigilantWebsitesInfectedTotal],
                    [VigilantWebsitesRecoveredTotal],
                    [NonVigilantWebsitesInfected],
                    [NonVigilantWebsitesRecovered],
                    [NonVigilantWebsitesInfectedTotal],
                    [NonVigilantWebsitesRecoveredTotal]
                )
                OUTPUT inserted.SimulationTurnId
                VALUES
                (
                    @SimulationId,
                    @TurnNo,
                    @VigilantUsersInfected,
                    @VigilantUsersRecovered,
                    @VigilantUsersInfectedTotal,
                    @VigilantUsersRecoveredTotal,
                    @NonVigilantUsersInfected,
                    @NonVigilantUsersRecovered,
                    @NonVigilantUsersInfectedTotal,
                    @NonVigilantUsersRecoveredTotal,
                    @VigilantWebsitesInfected,
                    @VigilantWebsitesRecovered,
                    @VigilantWebsitesInfectedTotal,
                    @VigilantWebsitesRecoveredTotal,
                    @NonVigilantWebsitesInfected,
                    @NonVigilantWebsitesRecovered,
                    @NonVigilantWebsitesInfectedTotal,
                    @NonVigilantWebsitesRecoveredTotal
                )";

            return sql.InsertQuery(query, queryParams);
        }


    }
}
