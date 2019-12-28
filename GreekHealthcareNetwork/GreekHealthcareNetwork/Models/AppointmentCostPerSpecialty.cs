using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    [Table("AppointmentCostPerSpecialty")]
    public class AppointmentCostPerSpecialty
    {
        [Key]
        public MedicalSpecialty MedicalSpecialty { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal AppointmentCost { get; set; }

        [JsonIgnore]
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}