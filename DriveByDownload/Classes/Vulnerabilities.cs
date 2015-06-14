using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveByDownload.Classes
{
    /// <summary>
    /// Describes a specific vulnerability, usually (but not necessarily) with a CVE Number, and the potential impacts of being compromised in this way.
    /// </summary>
    public class Vulnerability
    {
        Vulnerability(String cve)
        {
            this.CVENo = cve;
        }

        String CVENo { get; set; }
        //If it has a CVERating value, this can be used to affect the ChanceOfCompromise
        Double CVERating { get; set; }        
        Double EaseOfExploitation { get; set; }
        Double ChanceOfCompromise { get; set; }
        Boolean IsKnown { get; set; }
        int UnitsKnown { get; set; }
        

    }
}
