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
        public ActionResult BoockAppointment()
        {
            return View();
        }
    }
}