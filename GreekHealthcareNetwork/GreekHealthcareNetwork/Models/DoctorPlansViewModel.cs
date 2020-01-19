using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class DoctorPlansViewModel
    {
        public IEnumerable<DoctorPlan> DoctorPlans { get; set; }

        [Display(Name = "Medical Specialty")]
        public List<MedicalSpecialty> MedicalSpecialties { get; set; }
    }
}