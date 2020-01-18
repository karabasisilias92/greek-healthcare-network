using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly InsuredsRepository _insureds = new InsuredsRepository();
        public ActionResult Index(SearchLoginViewModel searchLoginViewModel)
        {
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.ReturnUrl = string.Empty;
            searchLoginViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for(int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchLoginViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            searchLoginViewModel.InsuredPlans = _insureds.GetInsuredPlans().ToList();
            return View(searchLoginViewModel);
        }
    }
}