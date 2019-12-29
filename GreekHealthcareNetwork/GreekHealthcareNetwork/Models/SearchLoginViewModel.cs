using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class SearchLoginViewModel
    {
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public int DoctorSpecialty { get; set; }
        public List<MedicalSpecialty> MedicalSpecialties { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DefaultValue(null)]
        public DateTime? AppointmentDate { get; set; }

        [Required]
        [Display(Name = "Username/Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}