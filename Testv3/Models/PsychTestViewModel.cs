using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Testv3.Models
{
    public class PsychTestViewModel
    {
        //PsychTestQuestion
        [Required]
        [Key]
        public int QuestionID { get; set; }
        [Required]
        public string Question { get; set; }

        //PsychTest
        public string UserID { get; set; }
        [Required]
        public int Answer { get; set; }
    }
}