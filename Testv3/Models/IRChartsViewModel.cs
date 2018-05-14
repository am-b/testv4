using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testv3.Models
{

    public class IRChartsViewModel
    {
        //IncidentReportIncidents list of incidents
        public string Type { get; set; }
        public int count { get; set; }

        public int counsellorCount { get; set; }
        public int studentCount { get; set; }



    }
}