using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testv3.Models
{
    public class StudentCounsellingViewModel
    {
        public string StudentUserID { get; set; }

        [Display(Name = "Student ID:")]
        public string StudentID { get; set; }

        public string StudentLastName { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }
        public string Program { get; set; }
        public Nullable<int> YearLevel { get; set; }

        public string CounsellorUserID { get; set; }
        public string CounsellorID { get; set; }
        public string CounsellorLastName { get; set; }
        public string CounsellorFirstName { get; set; }
        public string CounsellorMiddleName { get; set; }
        public string CounsellorEmail { get; set; }

        public int CounsellingFormID { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string Case { get; set; }
        public string Session { get; set; }
        public string PctionPlan { get; set; }
        public string Recommendation { get; set; }
        public string Followup { get; set; }

        public bool StudentAgrees { get; set; }


    }
}