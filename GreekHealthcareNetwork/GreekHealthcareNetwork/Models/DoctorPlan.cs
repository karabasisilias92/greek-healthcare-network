using System;

namespace GreekHealthcareNetwork.Models
{
    public class DoctorPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public MedicalSpecialty MedicalSpecialty { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d{2}))?$")]
        public decimal Fee { get; set; }
    }

}
