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
    
    public partial class InitialInterview
    {
        public int InitialInterviewID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string ReasonForProgram { get; set; }
        public string ReasonForMMCC { get; set; }
        public string CollegeLifeAdjustments { get; set; }
        public string ChoiceOfProgramAdjustments { get; set; }
        public string PeersAdjustments { get; set; }
        public string MMCCStaffAdjustments { get; set; }
        public string FamilyAdjustments { get; set; }
        public string CounselorNotes { get; set; }
    
        public virtual Student Student { get; set; }
    }
}