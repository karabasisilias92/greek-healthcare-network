using GreekHealthcareNetwork.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Doctor")]
        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [Required]
        [Display(Name = "Patient")]
        public string InsuredId { get; set; }

        [ForeignKey("InsuredId")]
        public virtual Insured Insured { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        [Column(TypeName = "money")]
        public decimal InsuredAppointmentCharge { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Appointment date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AppointmentDate { get; set; }

        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        [Required]
        [Display(Name = "Appointment time")]
        public TimeSpan AppointmentStartTime { get; set; }

        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentEndTime { get; set; }

        [Display(Name = "Appointment time")]
        [NotMapped]
        public string AppointmentTime 
        { get
            {
                return AppointmentStartTime.ToString(@"hh\:mm") + " - " + AppointmentEndTime.ToString(@"hh\:mm");
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public AppointmentStatus AppointmentStatus { get; set; }

        public bool AppointmentChargePaid { get; set; }

        [Display(Name = "Comments for doctor")]
        public string InsuredComments { get; set; }

        [Display(Name = "Comments for patient")]
        public string DoctorComments { get; set; }

        public bool PaidByCompany { get; set; }
    }
}