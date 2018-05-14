namespace Testv3.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppointmentModel : DbContext
    {
        public AppointmentModel()
            : base("name=AppointmentModel")
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .Property(e => e.Apptmnt_Time)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.StudentID)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.StudentEmail)
                .IsUnicode(false);
        }
    }
}
