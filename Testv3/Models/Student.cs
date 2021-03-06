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
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Answers = new HashSet<Answers>();
            this.InitialInterview = new HashSet<InitialInterview>();
            this.RoutineInterview = new HashSet<RoutineInterview>();
            this.CounsellingContract = new HashSet<CounsellingContract>();
            this.ExitInterview = new HashSet<ExitInterview>();
            this.AnecdotalRecord = new HashSet<AnecdotalRecord>();
            this.IncidentReport = new HashSet<IncidentReport>();
            this.CounsellingForm = new HashSet<CounsellingForm>();
            this.Appntmnt = new HashSet<Appntmnt>();
            this.IndividualInventoryRecord = new HashSet<IndividualInventoryRecord>();
        }
    
        public string UserID { get; set; }
        public string StudentID { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }
        public string StudentEmail { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string Civil_Status__CivilStatus { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string PhoneNumber { get; set; }
        public string Birthplace { get; set; }
        public string Dialect { get; set; }
        public string Hobbies { get; set; }
        public string BirthRank { get; set; }
        public string DistanceFromSchool { get; set; }
        public string Scholarship { get; set; }
        public Nullable<System.DateTime> DateOfMarriage { get; set; }
        public string PlaceOfMarriage { get; set; }
        public string SpouseName { get; set; }
        public string SpouseAge { get; set; }
        public string SpouseEducationalAttainment { get; set; }
        public string Occupation { get; set; }
        public string StudentEmployerAddress { get; set; }
        public string NumberOfChildren { get; set; }
        public Nullable<bool> IsScholar { get; set; }
        public string Program { get; set; }
        public Nullable<int> YearLevel { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answers> Answers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InitialInterview> InitialInterview { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoutineInterview> RoutineInterview { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CounsellingContract> CounsellingContract { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExitInterview> ExitInterview { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnecdotalRecord> AnecdotalRecord { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidentReport> IncidentReport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CounsellingForm> CounsellingForm { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appntmnt> Appntmnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IndividualInventoryRecord> IndividualInventoryRecord { get; set; }
    }
}
