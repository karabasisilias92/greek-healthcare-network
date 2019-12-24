using System;

namespace GreekHealthcareNetwork.Models
{
    public class Insured
    {
        [Key]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        [Column(Order=2)]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "InsuredPlan")]
        public int InsuredPlanId { get; set; }

        public virtual InsuredPlan InsuredPlan { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "HomeAddress must start with capital letter and then contain only small letters of the same language.")]
        public string HomeAddress { get; set; }

        public int BookAppointments { get; set; }


    }
}
