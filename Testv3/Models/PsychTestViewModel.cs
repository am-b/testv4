using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Testv3.Models
{
    public class PsychTestViewModel
    {
        //PsychTestQuestion Questions
        public int QuestionID { get; set; }
        public int QuestionnaireID { get; set; }
        [Required]
        public string Question { get; set; }

        //PsychTest Answers
        public string UserID { get; set; }
        public int AnswerID { get; set; }
        [Required(ErrorMessage = "Select your answer")]
        public int Answer { get; set; }

    }
}