using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreekHealthcareNetwork.Models
{
    public class DoctorPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Medical Specialty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MedicalSpecialty MedicalSpecialty { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        [Column(TypeName = "money")]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d{2}))?$")]
        [Display(Name = "Plan fee")]
        [Required]
        public decimal Fee { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        [Column(TypeName = "money")]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d{2}))?$")]
        [Display(Name = "Appointment Cost")]
        public decimal AppointmentCost { get; set; }

        [JsonIgnore]
        public virtual ICollection<Doctor> Doctors { get; set; }
    }

}
