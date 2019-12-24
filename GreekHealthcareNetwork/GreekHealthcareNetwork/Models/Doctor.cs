using GreekHealthcareNetwork.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class Doctor
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public MedicalSpecialty MedicalSpecialty { get; set; }

        [Required]
        public string OfficeAddress { get; set; }

        [Required]
        public string PaypalAccount { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal AppointmentCost { get; set; }

        [Required]
        public int DoctorPlanId { get; set; }

        [ForeignKey("DoctorPlanId")]
        public virtual DoctorPlan DoctorPlan { get; set; }

        public virtual ICollection<WorkingHours> WorkingHours { get; set; }

        public virtual ICollection<DoctorsUnavailability> DoctorsUnavailability { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }


    }
}