using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class DoctorPlan
    {
        [Key]
        public int Id { get; set; }
    }
}