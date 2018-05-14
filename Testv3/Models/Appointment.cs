namespace Testv3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appointment")]
    public partial class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int Apptmnt_ID { get; set; }

        [Column(TypeName = "date")]
        [Display(Name ="Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Please select date.")]
        public DateTime? Apptmnt_Date { get; set; }

        [StringLength(30)]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage ="Please Select Time")]
        public string Apptmnt_Time { get; set; }

        [StringLength(50)]
        [Display(Name = "Student ID")]
        public string StudentID { get; set; }

        [StringLength(256)]
        [Display(Name = "Student Email")]
        public string StudentEmail { get; set; }

    }
}
