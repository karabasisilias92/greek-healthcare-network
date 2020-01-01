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
    public class WorkingHours
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek? Day { get; set; }

        [Required]
        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Work Start Time")]
        public TimeSpan? WorkStartTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Work End Time")]
        public TimeSpan? WorkEndTime { get; set; }

        [Required]
        [RegularExpression(@"^15|30|45|60$", ErrorMessage = "Appointment duration must be 15, 30, 45 or 60 minutes.")]
        [Display(Name = "Appointment Duration")]
        public byte AppointmentDuration { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        [JsonIgnore]
        public virtual Doctor Doctor { get; set; }

        [NotMapped]
        public string WorkingDayConcat
        {
            get
            {
                return Day.ToString() + "," + " " + WorkStartTime.Value.Hours.ToString() + ":" + WorkStartTime.Value.Minutes.ToString("D2") + " " + "-" + " " + WorkEndTime.Value.Hours.ToString() + ":" + WorkEndTime.Value.Minutes.ToString("D2");
            }
        }
    }
}