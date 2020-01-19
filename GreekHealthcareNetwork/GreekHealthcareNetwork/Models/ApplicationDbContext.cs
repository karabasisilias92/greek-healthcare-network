using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<WorkingHours> WorkingHours { get; set; }
        public DbSet<DoctorsUnavailability> DoctorsUnavailabilities { get; set; }
        public DbSet<Insured> Insureds { get; set; }
        public DbSet<InsuredPlan> InsuredPlans { get; set; }
        public DbSet<DoctorPlan> DoctorPlans { get; set; }

        public ApplicationDbContext()
            : base("GHNConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                        .HasRequired(m => m.Sender)
                        .WithMany(s => s.Messages)
                        .HasForeignKey(m => m.SenderId)
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Message>()
                        .HasRequired(m => m.Recipient)
                        .WithMany()
                        .HasForeignKey(m => m.RecipientId)
                        .WillCascadeOnDelete(false);
        }
    }
}