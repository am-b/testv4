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
    
    public partial class Appointment
    {
        public int AppointmentID { get; set; }
        public Nullable<System.DateTime> AppointmentDate { get; set; }
        public Nullable<System.TimeSpan> AppointmentStartTime { get; set; }
        public Nullable<System.TimeSpan> AppointmentEndTime { get; set; }
        public string Remarks { get; set; }
        public string ReasonForVisit { get; set; }
        public Nullable<int> CounsellorID { get; set; }
        public Nullable<int> StudentID { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
