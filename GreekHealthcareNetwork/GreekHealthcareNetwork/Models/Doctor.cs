using GreekHealthcareNetwork.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        [JsonConverter(typeof(StringEnumConverter))]
        public MedicalSpecialty? MedicalSpecialty { get; set; }

        [ForeignKey("MedicalSpecialty")]
        public virtual AppointmentCostPerSpecialty AppointmentCost { get; set; }

        [Required]
        public string OfficeAddress { get; set; }

        [Required]
        public int DoctorPlanId { get; set; }

        [ForeignKey("DoctorPlanId")]
        [JsonIgnore]
        public virtual DoctorPlan DoctorPlan { get; set; }

        public virtual ICollection<WorkingHours> WorkingHours { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<DoctorsUnavailability> DoctorsUnavailability { get; set; }

        [JsonIgnore]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}