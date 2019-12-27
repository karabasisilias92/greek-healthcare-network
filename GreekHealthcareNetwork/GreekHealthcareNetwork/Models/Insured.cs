using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [JsonIgnore]
        public virtual InsuredPlan InsuredPlan { get; set; }

        [Required]
        [MaxLength(100)]
        public string HomeAddress { get; set; }

        public int BookedAppointments { get; set; }

        [JsonIgnore]
        public virtual ICollection<Appointment> Appointments { get; set; }


    }
}
