//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Testv3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class IncidentReport
    {
        public int IncidentReportID { get; set; }
        public string UserID { get; set; }
        public string StudentUserID { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string TypeOfIncident { get; set; }
        public string PlaceOfIncident { get; set; }
        public Nullable<System.DateTime> DateTimeOfIncident { get; set; }
        public string Witness { get; set; }
        public string Details { get; set; }
        public string CounsellorNotes { get; set; }
    
        public virtual Counsellor Counsellor { get; set; }
        public virtual Student Student { get; set; }
    }
}
