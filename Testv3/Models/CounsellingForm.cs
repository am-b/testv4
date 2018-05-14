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
    
    public partial class CounsellingForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CounsellingForm()
        {
            this.CounsellingFormCases = new HashSet<CounsellingFormCases>();
        }
    
        public int CounsellingFormID { get; set; }
        public string UserID { get; set; }
        public string StudentUserID { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string Session { get; set; }
        public string PctionPlan { get; set; }
        public string Recommendation { get; set; }
        public string Followup { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CounsellingFormCases> CounsellingFormCases { get; set; }
        public virtual Student Student { get; set; }
        public virtual Counsellor Counsellor { get; set; }
    }
}
