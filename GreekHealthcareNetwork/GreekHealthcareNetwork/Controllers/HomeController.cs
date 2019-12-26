using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(DoctorSearchViewModel doctorSearchViewModel)
        {
            doctorSearchViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for(int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                doctorSearchViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(doctorSearchViewModel);
        }
    }
}