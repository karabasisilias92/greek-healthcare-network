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
        public DateTime UnavailableFrom { get; set; }

        [Required]
        public DateTime UnavailableUntil { get; set; }

        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}