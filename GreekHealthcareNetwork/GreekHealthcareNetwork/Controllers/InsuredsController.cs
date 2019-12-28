using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    public class InsuredsController : Controller
    {
        // GET: Insureds
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BoockAppointment(SearchLoginViewModel searchViewModel)
        {
            searchViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(searchViewModel);
        }
    }
}