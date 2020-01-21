using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class AdminSearchViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DoctorSpecialty { get; set; }
        public int InsuredPlanId { get; set; }
        public List<MedicalSpecialty> MedicalSpecialties { get; set; }
        public List<InsuredPlan> InsuredPlans { get; set; }
    }
}