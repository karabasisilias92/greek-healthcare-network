using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class DoctorsUnavailability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name= "Unavailable From")]
        public DateTime UnavailableFrom { get; set; }

        [Required]
        [Display(Name = "Unavailable Until")]
        public DateTime UnavailableUntil { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}