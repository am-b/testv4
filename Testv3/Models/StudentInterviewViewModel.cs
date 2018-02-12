using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testv3.Models
{
    public class StudentInterviewViewModel
    {

        [Display(Name = "Student ID:")]
        public string StudentID { get; set; }

        public string StudentLastName { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }
        public string Program { get; set; }
        public Nullable<int> YearLevel { get; set; }


        public int InitialInterviewID { get; set; }
        public string UserID { get; set; }

        public Nullable<System.DateTime> CompletionDate { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "State your reason for enrolling in your course.")]
        public string ReasonForProgram { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "State your reasons for choosing Makati Medical Center College as your school.")]
        public string ReasonForMMCC { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "1. College life")]
        public string CollegeLifeAdjustments { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "2. Choice of Course")]
        public string ChoiceOfProgramAdjustments { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "3. Classmates and Peers")]
        public string PeersAdjustments { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "4. Teachers/Staffs of Makati Medical Center College")]
        public string MMCCStaffAdjustments { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "5. Family Concerns")]
        public string FamilyAdjustments { get; set; }

        public string CounselorNotes { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "What are the best year or moment in your life?")]
        public string Q1 { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "What are your inhibitions and fear now that your in your year level?")]
        public string Q2 { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "How are your difficulties with your academic requirements?")]
        public string Q3 { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "How are you coping up with your group mates and classmates?")]
        public string Q4 { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "What are the factors that greatly influence your performance at school?")]
        public string Q5 { get; set; }

        [Required(ErrorMessage = "Write your answer")]
        [Display(Name = "Other Matters")]
        public string OtherMatters { get; set; }
    }
}