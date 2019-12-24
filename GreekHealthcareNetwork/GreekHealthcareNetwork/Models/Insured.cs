using System;

namespace GreekHealthcareNetwork.Models
{
    public class Insured
    {
        [Key]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "InsuredPlan")]
        public int InsuredPlanId { get; set; }

        [ForeignKey("InsuredPlanId")]
        public virtual InsuredPlan InsuredPlan { get; set; }

        [Required]
        [MaxLength(100)]
        public string HomeAddress { get; set; }

        public int BookedAppointments { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }


    }
}
