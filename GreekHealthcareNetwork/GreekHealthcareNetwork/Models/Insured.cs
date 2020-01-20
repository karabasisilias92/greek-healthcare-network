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
        public int? InsuredPlanId { get; set; }

        [ForeignKey("InsuredPlanId")]
        public virtual InsuredPlan InsuredPlan { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Home Address length cannot be less than 10 characters.")]
        [MaxLength(100, ErrorMessage = "Home Address length cannot be more than 100 characters.")]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        public int BookedAppointments { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        [Column(TypeName = "money")]
        [Display(Name = "Refund pending")]
        public decimal RefundPending { get; set; }

        [JsonIgnore]
        public virtual ICollection<Appointment> Appointments { get; set; }


    }
}
