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
    
    public partial class IndividualInventoryRecord
    {
        public int RecordID { get; set; }
        public string UserID { get; set; }
        public string FathersName { get; set; }
        public string FathersAddress { get; set; }
        public string FathersAge { get; set; }
        public string FathersEducationalAttainment { get; set; }
        public string FathersOccupation { get; set; }
        public string FathersEmployerAddress { get; set; }
        public string MothersName { get; set; }
        public string MothersAddress { get; set; }
        public string MothersAge { get; set; }
        public string MothersEducationalAttainment { get; set; }
        public string MothersOccupation { get; set; }
        public string MothersEmployerAddress { get; set; }
        public string FamilyDwelling { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string ParentsStatus { get; set; }
        public string EconomicStatus { get; set; }
        public string NoOfSiblings { get; set; }
        public string PresentlyLivingWith { get; set; }
        public string PresentlyStayingAt { get; set; }
        public string ElementarySchool { get; set; }
        public string ElementaryAddress { get; set; }
        public string YearsAttendedElem { get; set; }
        public string HighSchool { get; set; }
        public string HighSchoolAddress { get; set; }
        public string YearsAttendedHS { get; set; }
        public string CollegeSchool { get; set; }
        public string CollegeAddress { get; set; }
        public string YearsAttendedCollege { get; set; }
        public string Honors { get; set; }
        public string FaveSubject { get; set; }
        public string LeastSubject { get; set; }
        public string HowStudieIssFinanced { get; set; }
        public Nullable<bool> IsCoursePersonalChoice { get; set; }
        public string CourseNotPersonalChoice { get; set; }
        public string CourseChoiceInfluence { get; set; }
        public string CoursePersonalChoice { get; set; }
        public string WhyMMCC { get; set; }
        public string ReferredToMMCCBy { get; set; }
        public string Position { get; set; }
        public string Salary { get; set; }
        public string Employer { get; set; }
        public string EmployerAddress { get; set; }
        public string EmploymentStatus { get; set; }
        public string MentalAbilityTestDate { get; set; }
        public string MentalAbilityTestScore { get; set; }
        public string MentalAbilityTestPercentile { get; set; }
        public string PersonalityTestDate { get; set; }
        public string PersonalityTestScore { get; set; }
        public string PersonalityTestPercentile { get; set; }
        public string VocationalTestDate { get; set; }
        public string VocationalTestScore { get; set; }
        public string VocationalTestPercentile { get; set; }
        public string Disabilities { get; set; }
        public string ChronicIllness { get; set; }
        public string PreviousAccidents { get; set; }
        public string PreviousSurgery { get; set; }
        public string MaintenanceMedicines { get; set; }
        public string Immunization { get; set; }
        public string HaveTalkedWithACounselor { get; set; }
        public string HaveTalkedWithACounselorWhen { get; set; }
        public string HaveTalkedWithACounselorWhy { get; set; }
        public string HaveTalkedWithAPsychiatrist { get; set; }
        public string HaveTalkedWithAPsychiatristWhen { get; set; }
        public string HaveTalkedWithAPsychiatristWhy { get; set; }
        public string HaveTalkedWithAPsychologist { get; set; }
        public string HaveTalkedWithAPsychologistWhen { get; set; }
        public string HaveTalkedWithAPsychologistWhy { get; set; }
        public string AboutYourself { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
