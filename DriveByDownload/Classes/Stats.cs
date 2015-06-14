using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    public class Stats
    {
        private static String path = @"C:\Users\Huw\Dropbox\THESISv2\results\results-folder\";

        public static List<SqlParameter> GetSimulationTurnLowHigh(String condition)
        {
            SQL sql = new SQL();
            DataTable dt = sql.SelectQuery("SELECT Low,High FROM SimulationTurnMinMax WHERE Condition = @condition",
                new List<SqlParameter> {new SqlParameter("condition", condition)});
            return new List<SqlParameter> { new SqlParameter("low", (int)dt.Rows[0][0]), new SqlParameter("high", (int)dt.Rows[0][1]) };
        }

        public static void InterventionLevelsANOVA(String filename, int column)
        {
            SQL sql = new SQL();
            String[] conditions = { "EU", "US" };
            //var dataTableDictionary = new Dictionary<string, Dictionary<double, DataTable>>();
            var sb = new StringBuilder();
            sb.AppendLine("Country,Compliance,InfectedNonVigilantUsers");
            for (double i = 1.0; i > 0.1; i -= 0.2)
            {
                //System.Diagnostics.Debug.WriteLine(i);
                foreach (var s in conditions)
                {
                    String cond = "intervention-" + s + "-" + String.Format("{0:0.0}",i) + "-compliance";
                    System.Diagnostics.Debug.WriteLine(cond);
                    var table = sql.SelectQuery("reporting_getTurn150",
                        new List<SqlParameter> {new SqlParameter("condition", cond)}, true);
                    for(int j = 0; j < table.Rows.Count; j++)
                    {
                        int codedCountry = s == "EU" ? 1 : 2;
                        sb.AppendLine(codedCountry + "," + String.Format("{0:0.0}", i) + "," + table.Rows[j][column]);
                    }
                    
                }
            }

            sb = AddDefaultCondition(sb, column);

            StreamWriter writetext = new StreamWriter(path + filename);
            writetext.Write(sb.ToString());
            writetext.Close();
        }

        public static void VigilanceANOVA(String filename, int column)
        {
            SQL sql = new SQL();
            var sb = new StringBuilder("WebsiteVigilance,UserVigilance,TotalInfections\n");
            for (double d = 1.0; d > 0.1; d -= 0.2)
            {
                for (double e = 1.0; e > 0.1; e -= 0.2)
                {
                    String cond = "vig-web-" + String.Format("{0:0.0}", d) + "-vig-user-" + String.Format("{0:0.0}", e);
                    DataTable table = sql.SelectQuery("reporting_getTurn150",
                        new List<SqlParameter> {new SqlParameter("condition", cond)}, true);
                    System.Diagnostics.Debug.WriteLine(cond);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        //This one usually
                        //sb.AppendLine(String.Format("{0:0.0}", d) + "," + String.Format("{0:0.0}", e) + "," + table.Rows[i][column]);
                        //Use this one to add to the bottom of VigilanceInterventionANOVA
                        sb.AppendLine("1," + "," + String.Format("{0:0.0}", d) + "," + String.Format("{0:0.0}", e) + ",1.0," + table.Rows[i][column]);
                    }
                }
            }
            //sb = AddDefaultCondition(sb, column);


            var writetext = new StreamWriter(path + filename);
            writetext.Write(sb.ToString());
            writetext.Close();
        }

        public static void NewVigilanceInterventionANOVA(String filename, int column)
        {
            SQL sql = new SQL();
            var sb = new StringBuilder("Condition,CompliantHosts,TotalInfections\n");
            List<String> conditions = new List<string>
                        {
                            "vig-web-0.8-vig-user-0.2",
                            "vig-web-0.2-vig-user-0.8",
                            "vig-web-0.8-vig-user-0.8",
                            "vig-web-0.2-vig-user-0.2"
                        };

            for(int i = 0; i < conditions.Count; i++)
            {
                String s = conditions[i];
                List<SqlParameter> sqlParams = GetSimulationTurnLowHigh(s);
                sqlParams.Add(new SqlParameter("condition", s));
                System.Diagnostics.Debug.WriteLine(s);
                DataTable table = sql.SelectQuery("reporting_getTurn150", sqlParams, true);
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    sb.AppendLine(i + "," + 0 + "," + table.Rows[j][column]);
                }
                for (double d = 1.0; d > 0.1; d -= 0.2)
                {
                    String c = s.Replace("vig-web", "VW");
                    c = c.Replace("vig-user", "VU");
                    String cond = "intervention-EU-" + String.Format("{0:0.0}", d) + "-compliance-" + c;
                    System.Diagnostics.Debug.WriteLine(cond);
                    sqlParams = GetSimulationTurnLowHigh(cond);
                    sqlParams.Add(new SqlParameter("condition",cond));

                    sqlParams[2] = new SqlParameter("condition", cond);
                    table = sql.SelectQuery("reporting_getTurn150", sqlParams, true);
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        sb.AppendLine(i + "," + String.Format("{0:0.0}", d) + "," + table.Rows[j][column]);
                    }
                }
            }

            StreamWriter writetext = new StreamWriter(path + filename);
            writetext.Write(sb.ToString());
            writetext.Close();
        }

        public static void VigilanceInterventionANOVA(String filename, int column)
        {
            SQL sql = new SQL();
            var sb = new StringBuilder("Country,WebsiteVigilance,UserVigilance,CompliantHosts,TotalInfections\n");
            //for (double d = 1.0; d > 0.1; d -= 0.2)
            //{
                for (double d = 1.0; d > 0.1; d -= 0.2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        List<String> conditions = new List<string>
                        {
                            "vig-web-0.8-vig-user-0.2",
                            "vig-web-0.2-vig-user-0.8",
                            "vig-web-0.8-vig-user-0.8",
                            "vig-web-0.2-vig-user-0.2"
                        };
                        
                        if (i == 1)
                        {
                            conditions[0] = "intervention-EU-" + String.Format("{0:0.0}", d) + "-compliance-" + conditions[0];
                            conditions[1] = "intervention-EU-" + String.Format("{0:0.0}", d) + "-compliance-" + conditions[1];
                            conditions[2] = "intervention-EU-" + String.Format("{0:0.0}", d) + "-compliance-" + conditions[2];
                            conditions[3] = "intervention-EU-" + String.Format("{0:0.0}", d) + "-compliance-" + conditions[3];
                        }
                        //We only want those four conditions to be done once...
                        else if (d <= 0.8)
                        {
                            continue;
                        }

                        for (int j = 0; j < conditions.Count; j++)
                        {
                            if (i == 1)
                            {
                                conditions[j] = conditions[j].Replace("vig-web", "VW");
                                conditions[j] = conditions[j].Replace("vig-user", "VU");
                            }
                            List<SqlParameter> sqlParams = GetSimulationTurnLowHigh(conditions[j]);
                            sqlParams.Add(new SqlParameter("condition", conditions[j]));
                            System.Diagnostics.Debug.WriteLine(conditions[j]);
                            
                            DataTable table = sql.SelectQuery("reporting_getTurn150",sqlParams, true);                            
                            for (int k = 0; k < table.Rows.Count; k++)
                            {
                                String vigWeb = "";
                                String vigUser = "";
                                switch (j)
                                {
                                    case 0:
                                        vigWeb = "0.8";
                                        vigUser = "0.2";
                                        break;
                                    case 1:
                                        vigWeb = "0.2";
                                        vigUser = "0.8";
                                        break;
                                    case 2:
                                        vigWeb = "0.8";
                                        vigUser = "0.8";
                                        break;
                                    case 3:
                                        vigWeb = "0.2";
                                        vigUser = "0.2";
                                        break;
                                }
                                sb.AppendLine(i + "," + vigWeb + "," + vigUser + "," + String.Format("{0:0.0}", d) + "," + table.Rows[k][column]);

                            }
;
                        }
                    }                    
                }

            //}

            StreamWriter writetext = new StreamWriter(path + filename);
            writetext.Write(sb.ToString());
            writetext.Close();
        }

        public static void FullVigilanceInterventionANOVA(String filename, int column)
        {
            SQL sql = new SQL();
            var sb = new StringBuilder("Country,WebsiteVigilance,UserVigilance,CompliantHosts,TotalInfections\n");
            for (double c = 1.0; c > 0.1; c -= 0.2)
            {
                for (double d = 1.0; d > 0.1; d -= 0.2)
                {
                    for (double e = 1.0; e > 0.1; e -= 0.2)
                    {
                        String cond = "intervention-EU-" + String.Format("{0:0.0}", c) + "-compliance-VW-" +
                                             String.Format("{0:0.0}", d) + "-VU-" + String.Format("{0:0.0}", e);
                        var sqlParams = GetSimulationTurnLowHigh(cond);
                        sqlParams.Add(new SqlParameter("condition", cond));
                        DataTable table = sql.SelectQuery("reporting_getTurn150", sqlParams, true);
                        System.Diagnostics.Debug.WriteLine(cond);

                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            
                            sb.AppendLine("1,"+ String.Format("{0:0.0}", c) + "," + String.Format("{0:0.0}", d) + "," + String.Format("{0:0.0}", e) + "," + table.Rows[i][column]);
                        }
                    }
                }
            }

            StreamWriter writetext = new StreamWriter(path + filename);
            writetext.Write(sb.ToString());
            writetext.Close();

        }

        //Needed for comparisons against control in SPSS.  The group and value will ALWAYS be 0, so be careful about that...
        public static StringBuilder AddDefaultCondition(StringBuilder sb, int column)
        {
            SQL sql = new SQL();
            var dt1 = sql.SelectQuery("reporting_getTurn150", new List<SqlParameter> { new SqlParameter("condition", "default") }, true);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                sb.AppendLine("0,0," + dt1.Rows[i][column]);
            }
            return sb;
        }

        public static void Turn150Results(String filename, int column, bool R)
        {
            StreamWriter writetext = new StreamWriter(path + filename);

            SQL sql = new SQL();
            String[] conditions = { "default", "world", "US", "EU", "UK" };
            var dt1 = sql.SelectQuery("reporting_getTurn150", new List<SqlParameter> {new SqlParameter("condition", "default")}, true);
            var dt2 = sql.SelectQuery("reporting_getTurn150", new List<SqlParameter> { new SqlParameter("condition", "intervention-UK-1.0-compliance") }, true);
            var dt3 = sql.SelectQuery("reporting_getTurn150", new List<SqlParameter> { new SqlParameter("condition", "intervention-US-1.0-compliance") }, true);
            var dt4 = sql.SelectQuery("reporting_getTurn150", new List<SqlParameter> { new SqlParameter("condition", "intervention-EU-1.0-compliance") }, true);
            var dt5 = sql.SelectQuery("reporting_getTurn150", new List<SqlParameter> { new SqlParameter("condition", "intervention-World-1.0-compliance") }, true);
            

            List<DataTable> tables = new List<DataTable>{dt1,dt2,dt3,dt4,dt5};
            StringBuilder sb = new StringBuilder();
            //if (dt1.Rows.Count > 0 && dt2.Rows.Count > 0 && dt3.Rows.Count > 0 && dt4.Rows.Count > 0 &&
            //    dt5.Rows.Count > 0)
            //{
                
                if (R)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        //For R, do this
                        //String line = dt1.Rows[i][1].ToString() + "," + dt2.Rows[i][1].ToString() + "," +
                         //             dt3.Rows[i][1].ToString() + "," +
                        //              dt4.Rows[i][1].ToString() + "," + dt5.Rows[i][1].ToString();
                       // sb.AppendLine(line);


                    }
                }
                else
                {
                    int factor = 1;
                    //System.Diagnostics.Debug.WriteLine("Here..");
                    //For SPSS, do this:
                    foreach (var dt in tables)
                    {
                        //System.Diagnostics.Debug.WriteLine("Here loop..");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //System.Diagnostics.Debug.WriteLine("Here inner loop..");
                            sb.AppendLine(factor + "," + dt.Rows[i][column]);
                        }
                        factor++;
                    }
                }
                
            //}

            writetext.Write(sb.ToString());
            writetext.Close();
            //return dt;
        }



    }
}
