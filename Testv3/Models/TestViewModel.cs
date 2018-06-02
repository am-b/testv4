using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testv3.Models
{
    public class TestViewModel

    {
        public string UserID { get; set; }
        [Display(Name = "Student ID:")]
        public string StudentID { get; set; }

        public string StudentLastName { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }

        [Display(Name = "Email Address:")]
        public string StudentEmail { get; set; }
        public Nullable<int> CourseID { get; set; }
        public string Address { get; set; }

        [Required]
        public string Sex { get; set; }
        public IEnumerable<SelectListItem> Sexx { get; set; }

        [Required]
        [Display(Name = "Civil Status")]
        public string Civil_Status__CivilStatus { get; set; }
        public IEnumerable<SelectListItem> Civil_Status__CivilStatuss { get; set; }

        [Required]
        public string Religion { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        public Nullable<System.DateTime> Birthdate { get; set; }
        [Display(Name = "Phone Number:")]
        public string PhoneNumber { get; set; }
        public string Birthplace { get; set; }
        [Display(Name = "Languages and Dialect:")]
        public string Dialect { get; set; }
        [Display(Name = "Hobbies and Interests:")]
        public string Hobbies { get; set; }
        [Display(Name = "Birth Rank:")]
        public string BirthRank { get; set; }
        [Display(Name = "Distance of school from home:")]
        public string DistanceFromSchool { get; set; }

        [Display(Name = "Recipient of financial aid/scholarships?")]
        [Required]
        public bool IsScholar { get; set; }
        public string Scholarship { get; set; }


        [Display(Name = "Date Of Marriage:")]
        public Nullable<System.DateTime> DateOfMarriage { get; set; }
        [Display(Name = "Place Of Marriage:")]
        public string PlaceOfMarriage { get; set; }
        [Display(Name = "Spouse Name:")]
        public string SpouseName { get; set; }
        [Display(Name = "Age:")]
        public string SpouseAge { get; set; }

        [Display(Name = "Educational Attainment:")]
        public string SpouseEducationalAttainment { get; set; }
        public IEnumerable<SelectListItem> SpouseEducationalAttainments { get; set; }


        public string Occupation { get; set; }
        [Display(Name = "Employer Address:")]
        public string StudentEmployerAddress { get; set; }
        [Display(Name = "Number Of Children:")]
        public string NumberOfChildren { get; set; }
        [Display(Name = "Father's Name:")]
        public string FathersName { get; set; }
        [Display(Name = "Address:")]
        public string FathersAddress { get; set; }
        [Display(Name = "Age:")]
        public string FathersAge { get; set; }
        [Display(Name = "Educational Attainment:")]
        public string FathersEducationalAttainment { get; set; }
        public IEnumerable<SelectListItem> FathersEducationalAttainments { get; set; }

        [Display(Name = "Occupation:")]
        public string FathersOccupation { get; set; }
        [Display(Name = "Employer Address:")]
        public string FathersEmployerAddress { get; set; }
        [Display(Name = "Mother's Name:")]
        public string MothersName { get; set; }
        [Display(Name = "Address:")]
        public string MothersAddress { get; set; }
        [Display(Name = "Age:")]
        public string MothersAge { get; set; }

        [Display(Name = "EducationalAttainment :")]
        public string MothersEducationalAttainment { get; set; }
        public IEnumerable<SelectListItem> MothersEducationalAttainments { get; set; }
        [Display(Name = "Occupation:")]
        public string MothersOccupation { get; set; }
        [Display(Name = "Employer Address:")]
        public string MothersEmployerAddress { get; set; }
        [Display(Name = "Family Dwelling:")]
        public string FamilyDwelling { get; set; }
        public IEnumerable<SelectListItem> FamilyDwellings { get; set; }

        [Required]
        [Display(Name = "Emergency Contact Name:")]
        public string EmergencyContactName { get; set; }

        [Required]
        [Display(Name = "Emergency Contact Number:")]
        public string EmergencyContactNumber { get; set; }

        [Display(Name = "Status of Parents:")]
        public string ParentsStatus { get; set; }
        public IEnumerable<SelectListItem> ParentsStatuses { get; set; }

        [Display(Name = "Economic Status (parent's combined monthly income) :")]
        public string EconomicStatus { get; set; }
        public IEnumerable<SelectListItem> EconomicStatuses { get; set; }

        [Display(Name = "Number Of Siblings:")]
        public string NoOfSiblings { get; set; }

        
        public string PresentlyLivingWith { get; set; }
        public IEnumerable<SelectListItem> PresentlyLivingWiths { get; set; }

       
        public string PresentlyStayingAt { get; set; }
        public IEnumerable<SelectListItem> PresentlyStayingAts { get; set; }

        [Display(Name = "Elementary School:")]
        public string ElementarySchool { get; set; }
        [Display(Name = "Address:")]
        public string ElementaryAddress { get; set; }
        [Display(Name = "Years Attended:")]
        public string YearsAttendedElem { get; set; }
        [Display(Name = "Highschool:")]
        public string HighSchool { get; set; }
        [Display(Name = "Address:")]
        public string HighSchoolAddress { get; set; }
        [Display(Name = "Years Attended:")]
        public string YearsAttendedHS { get; set; }
        [Display(Name = "College:")]
        public string CollegeSchool { get; set; }
        [Display(Name = "Address:")]
        public string CollegeAddress { get; set; }
        [Display(Name = "Years Attended:")]
        public string YearsAttendedCollege { get; set; }

        //others
        [Display(Name = "Honors and Awards received:")]
        public string Honors { get; set; }

        [Display(Name = "Favorie subject/s in highschool:")]
        public string FaveSubject { get; set; }

        [Display(Name = "Least favorie subject/s in highschool:")]
        public string LeastSubject { get; set; }
        
        public string HowStudieIssFinanced { get; set; }

        [Required]
        public bool IsCoursePersonalChoice { get; set; }

       
        public string CourseNotPersonalChoice { get; set; }

        [Required]
        public string CourseChoiceInfluence { get; set; }
        public IEnumerable<SelectListItem> CourseChoiceInfluence_Dropdown { get; set; }

        
        public string CoursePersonalChoice { get; set; }

        [Required]
        public string WhyMMCC { get; set; }
        public IEnumerable<SelectListItem> WhyMMCC_Dropdown { get; set; }

        [Required]
        public string ReferredToMMCCBy { get; set; }
        public IEnumerable<SelectListItem> ReferredToMMCCBy_Dropdown { get; set; }



        public string Position { get; set; }
        public string Salary { get; set; }
        public string Employer { get; set; }
        public string EmployerAddress { get; set; }
        public string EmploymentStatus { get; set; }
        public IEnumerable<SelectListItem> EmploymentStatuses { get; set; }


        [Display(Name = "Mental Ability Test Date:")]
        public string MentalAbilityTestDate { get; set; }
        [Display(Name = "Mental Ability Test Score:")]
        public string MentalAbilityTestScore { get; set; }
        [Display(Name = "Mental Ability Test Percentile:")]
        public string MentalAbilityTestPercentile { get; set; }
        [Display(Name = "Personality Test Date:")]
        public string PersonalityTestDate { get; set; }
        [Display(Name = "Personality Test Score:")]
        public string PersonalityTestScore { get; set; }
        [Display(Name = "Personality Test Percentile:")]
        public string PersonalityTestPercentile { get; set; }
        [Display(Name = "Vocational Test Date:")]
        public string VocationalTestDate { get; set; }
        [Display(Name = "Vocational Test Score:")]
        public string VocationalTestScore { get; set; }
        [Display(Name = "Vocational Test Percentile:")]
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

        [Display(Name = "About the student:")]
        [Required]
        public string AboutYourself { get; set; }

        public Nullable<System.DateTime> CompletionDate { get; set; }

    }
}
