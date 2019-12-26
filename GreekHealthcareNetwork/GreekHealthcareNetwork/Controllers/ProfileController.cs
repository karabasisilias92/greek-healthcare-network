using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace GreekHealthcareNetwork.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();
            return View();
        }

        public ActionResult AppointmentsHistory()
        {
            return View();
        }
    }
}