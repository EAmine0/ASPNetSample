using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tryone.Models
{
    public class OpDashboard
    {
        //------------------------------Clinical Operational

        public class SiteIdentifiedPerCountry
        {
            public string country { get; set; }
            public int site_identified { get; set; }

        }

        public class Sites
        {
            public int total_value { get; set; }
            public int potential_value { get; set; }

        }

        public class Patients
        {
            public int total_value { get; set; }
            public int potential_value { get; set; }

        }

        public class SiteStatus
        {
            public string label { get; set; }
            public int status_total { get; set; }
            public int last_status_total { get; set; }

        }

        public class PatientStatus
        {
            public string label { get; set; }
            public int status_total { get; set; }
            public int last_status_total { get; set; }

        }

        public class CurveOfInclusion
        {
            public string date { get; set; }
            public int included { get; set; }
            public int randomised { get; set; }
            public int theoretical { get; set; }
        }

        public class Monitoring
        {
            public string nature { get; set; }
            public string mode { get; set; }
            public int last_status { get; set; }
            public float avg_monitoring_per_site { get; set; }

        }

        public class Documents
        {
            public string no_yes { get; set; }
            public int value { get; set; }
            public int received { get; set; }
            public int default_unresolved { get; set; }
        }

        public class Safety
        {
            public string initial_followup { get; set; }
            public int value { get; set; }
            public int value2 { get; set; }
            public int ack_not_received { get; set; }
        }
}
