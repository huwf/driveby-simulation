using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DriveByDownload.Classes
{
    public class Country
    {
        private Dictionary<double, Host> _hosts;
        public Dictionary<double, Host> Hosts { get { return _hosts; } set { _hosts = value; } }
        //private SQL sql;

        public Country(String name, int id, Boolean scans = false)
        {
            this.Name = name;
            this.Scans = scans;
            this.Id = id;
            this.SetHostDict();
            this.HostList = Hosts.Values.ToList();
            //sql = new SQL();
        }

        public String Name { get; set; }
        public Boolean Scans { get; set; }
        public int Id { get; set; }
        public List<Host> HostList { get; set; }

        private void SetHostDict()
        {  
            Dictionary<double, Host> Hosts = new Dictionary<double, Host>();
            double TotalShare = 0;
            String CountryString = "";
            String query = "SELECT HostName, MarketShare FROM HostingProviders WHERE Country = @Country";
            SqlParameter param = new SqlParameter("Country", this.Id);
            SQL sql = new SQL();
            DataTable dt = sql.SelectQuery(query, new List<SqlParameter> { param });
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Boolean compliant = Utility.SetBoolByProbability(Globals.COMPLIANT_HOSTS);
                    String HostName = (String)row[0];
                    double MarketShare = (double)row[1];
                    //CountryString = (String)row[2];
                    TotalShare += MarketShare;
                    
                    Hosts.Add(TotalShare, new Host(HostName, this, compliant));
                }
            }
            //Assume that any "smaller" hosts won't individually have an impact, so we can set this to false.
            Hosts.Add(1.0, new Host("Other" + CountryString, this, false));
            this.Hosts = Hosts;
        }
    }
}
