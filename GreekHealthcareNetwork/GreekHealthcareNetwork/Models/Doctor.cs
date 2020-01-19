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
        [Display(Name = "Medical Specialty")]
        public MedicalSpecialty? MedicalSpecialty { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Office Address length cannot be less than 10 characters.")]
        [MaxLength(100, ErrorMessage = "Office Address length cannot be more than 100 characters.")]
        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }

        [Required]
        public int DoctorPlanId { get; set; }

        [ForeignKey("DoctorPlanId")]
        [JsonIgnore]
        public virtual DoctorPlan DoctorPlan { get; set; }

        [Display(Name = "Working Hours")]
        public virtual ICollection<WorkingHours> WorkingHours { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<DoctorsUnavailability> DoctorsUnavailability { get; set; }

        [JsonIgnore]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}