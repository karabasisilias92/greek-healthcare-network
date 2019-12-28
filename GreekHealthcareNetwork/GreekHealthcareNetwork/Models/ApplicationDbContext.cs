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
        public DbSet<AppointmentCostPerSpecialty> AppointmentCostPerSpecialty {get; set;}

        public ApplicationDbContext()
            : base("GCNConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}