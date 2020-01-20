using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreekHealthcareNetwork.Models
{
    public class InsuredPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]  
        public string Name { get; set; }

        [Required]
        [Display(Name = "Plan Appointments")]
        public string PlanAppoinments { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d{2}))?$")]
        [Column(TypeName = "money")]
        [Display(Name = "Plan fee")]
        public decimal PlanFee { get; set; }

        [Required]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d+))?$")]
        [Display(Name = "Appointment Cost Rate %")]
        public double AppointmentRate { get; set; }

        [Required]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d+))?$")]
        [Display(Name = "Appointment Cost Rate % (over limit)")]
        public double ExceededAppointmentRate { get; set; }

        [Required]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d+))?$")]
        [Display(Name = "Cancellation Refund % Percentage")]
        public double CancelationRefundPercentage { get; set; }

        [Required]
        [Display(Name = "Plan Duration (months)")]
        public int PlanDuration { get; set; }

    }
}
