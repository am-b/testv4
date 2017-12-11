using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testv3.Models
{
    public partial class StudentDropdown
    {
        [Key]
        public string StudentEmail { get; set; }

        public string StudentLastName { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }

        [Required]
        public string Address { get; set; }
        
        public string Sex { get; set; }
        public IEnumerable<SelectListItem> Sexx { get; set; }

        public string Civil_Status__CivilStatus { get; set; }
        public IEnumerable<SelectListItem> Civil_Status__CivilStatuss { get; set; }

        //[Required]
        public string Religion { get; set; }

        //[Required]
        public string Nationality { get; set; }

        //[Required]
        public Nullable<System.DateTime> Birthdate { get; set; }

        //[Required]
        public Nullable<decimal> PhoneNumber { get; set; }

        //[Required]
        public string Birthplace { get; set; }
        public string Dialect { get; set; }
        public string Hobbies { get; set; }

        //[Required]
        public string BirthRank { get; set; }
        public string DistanceFromSchool { get; set; }
        public string Scholarship { get; set; }

    }
}