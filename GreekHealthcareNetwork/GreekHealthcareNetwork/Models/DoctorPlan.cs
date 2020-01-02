using System;
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
        public MedicalSpecialty MedicalSpecialty { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d{2}))?$")]
        [Display(Name = "Plan fee")]
        public decimal Fee { get; set; }
    }

}
