using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    public class CMS
    {
        public CMS(String name, String version)
        {
            this.Name = name;
            this.Version = version;
            Websites = new List<Website>();
        }

        public String Name { get; set; }
        public String Version { get; set; }
        public List<Plugin> Plugins { get; set; }
        public Boolean IsVulnerable { get; set; }
        public List<Website> Websites{ get; set; }

    }
}
