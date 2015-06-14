using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    public class Plugin
    {
        public Plugin(String name, String version)
        {
            this.Name = name;
            this.Version = version;
        }
        String Name { get; set; }
        String Version { get; set; }
        List<Vulnerability> Vulnerabilities { get; set; }
    }
}
