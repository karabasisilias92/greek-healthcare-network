using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class PaySubcriptionViewModel
    {
        [Required]
        public string UserId { get; set; }

        public int PlanId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "Plan Fee")]
        public decimal PlanFee { get; set; }
    }
}