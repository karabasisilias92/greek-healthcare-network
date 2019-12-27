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

        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public string InsuredId { get; set; }

        [ForeignKey("InsuredId")]
        public virtual Insured Insured { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentStartTime { get; set; }

        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentEndTime { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AppointmentStatus AppointmentStatus { get; set; }

        public string InsuredComments { get; set; }

        public string DoctorComments { get; set; }
    }
}