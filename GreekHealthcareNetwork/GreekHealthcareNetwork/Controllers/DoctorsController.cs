using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorsController : Controller
    {
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        // GET: Doctors
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Appointments()
        {
            return View();
        }

        public ActionResult Unavailabilities()
        {
            IEnumerable<DoctorsUnavailability> doctorsUnavailabilities = _doctors.GetUnavailabilities(User.Identity.GetUserId());
            return View(doctorsUnavailabilities);
        }        
    }
}