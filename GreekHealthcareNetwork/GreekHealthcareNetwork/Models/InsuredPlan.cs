using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreekHealthcareNetwork.Models
{
    public class InsuredPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]  
        public string Name { get; set; }

        [Required]
        public string PlanAppoinments { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal PlanFee { get; set; }

        [Required]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d+))?$")]
        public double AppointmentRate { get; set; }

        [Required]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d+))?$")]
        public double ExceededAppointmentRate { get; set; }

        [Required]
        public int PlanDuration { get; set; }

    }
}
