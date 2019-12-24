using System;

namespace GreekHealthcareNetwork.Models
{
    class InsuredPlan
    {
        [PrimaryKey]
        public int InsuredPlanId { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "Name must start with capital letter and then contain only small letters of the same language.")]
        public string Name { get; set; }

        public string PlanAppoinments { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "Fee")]
        public decimal PlanFee { get; set; }

        [Required]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d+))?$")]
        public double AppointmentRate { get; set; }
    }
}
