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
    
    public partial class ExitInterview
    {
        public int ExitInterviewID { get; set; }
        public string StudentUserID { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string MMCCLikes { get; set; }
        public string MMCCDislikes { get; set; }
        public string MMCCMoments { get; set; }
        public string Professors { get; set; }
        public string Staff { get; set; }
        public string Future { get; set; }
        public string Others { get; set; }
        public string GuidanceNotes { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
