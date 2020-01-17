using Newtonsoft.Json;
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
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name= "Unavailable From Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime UnavailableFromDate { get; set; }

        [Required]
        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        [Display(Name = "Unavailable From Time")]
        public TimeSpan UnavailableFromTime { get; set; }

        [Display(Name = "Unavailable From")]
        [NotMapped]
        public string UnavailableFrom
        {
            get
            {
                return UnavailableFromDate.ToString("dd-MM-yyyy") + " " + UnavailableFromTime.ToString(@"hh\:mm");
            }
        }

        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Unavailable Until Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime UnavailableUntilDate { get; set; }

        [Required]
        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        [Display(Name = "Unavailable Until Time")]
        public TimeSpan UnavailableUntilTime { get; set; }

        [Display(Name = "Unavailable Until")]
        [NotMapped]
        public string UnavailableUntil
        {
            get
            {
                return UnavailableUntilDate.ToString("dd-MM-yyyy") + " " + UnavailableUntilTime.ToString(@"hh\:mm");
            }
        }

        [Required]
        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        [JsonIgnore]
        public virtual Doctor Doctor { get; set; }
    }
}