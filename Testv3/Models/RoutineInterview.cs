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
    
    public partial class RoutineInterview
    {
        public int RoutineInterviewID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string OtherMatters { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
