using GreekHealthcareNetwork.Models.Enums;
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

        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public int InsuredId { get; set; }

        [ForeignKey("InsuredId")]
        public virtual Insured Insured { get; set; }

        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime AppointmentStartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime AppointmentEndTime { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }

        public string InsuredComments { get; set; }

        public string DoctorComments { get; set; }
    }
}