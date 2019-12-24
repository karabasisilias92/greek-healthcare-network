using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class WorkingHours
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        public TimeSpan WorkStartTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        public TimeSpan WorkEndTime { get; set; }

        [Required]
        [RegularExpression(@"^15|30|45|60$", ErrorMessage = "Appointment duration must be 15, 30, 45 or 60 minutes.")]
        public byte AppointmentDuration { get; set; }

        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}