using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    public class Browser
    {
        //TODO: Find lists of vulnerabilities
        public Browser(String name, String version)
        {
            this.Name = name;
            this.Version = version;
            //Get the rest of the information from a database.
        }

        public String Name { get; set; }
        public String Version { get; set; }
        //public DateTime ReleaseDate { get; set; }
        public Boolean IsVulnerable { get; set; }
        public List<Vulnerability> Vulnerabilities { get; set; }
        public Double AttackSuccessProbability { get; set; }
        
    }
}
