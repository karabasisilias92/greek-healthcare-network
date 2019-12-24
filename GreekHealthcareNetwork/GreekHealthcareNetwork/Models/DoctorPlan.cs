using System;

namespace GreekHealthcareNetwork.Models
{
    public class DoctorPlan
    {
        [PrimaryKey]
        public int DoctorId { get; set; }

        [Required]
        public Specility Specility { get; private set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "Fee")]
        [RegularExpression(@"^(([1-9]\d+)|\d)(\.(\d{2}))?$")]
        public decimal Fee { get; set; }
    }

}
