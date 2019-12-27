using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class SearchViewModel
    {
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public int DoctorSpecialty { get; set; }
        public List<MedicalSpecialty> MedicalSpecialties { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}