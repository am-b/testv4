using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testv3.Models
{

    public class IR2ViewModel
    {
        //IncidentReportIncidents list of incidents
        public string Type { get; set; }

        //IncidentReportTags yung mga selected tags
        public int TagID { get; set; }
        public int IncidentReportID { get; set; }
        public int TypeID { get; set; }



    }
}