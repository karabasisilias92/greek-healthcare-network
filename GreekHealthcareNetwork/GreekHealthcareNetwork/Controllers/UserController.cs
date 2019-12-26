using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace GreekHealthcareNetwork.Controllers
{
    public class UserController : Controller
    {
        // GET: Profile
        public ActionResult Profile()
        {
            //var userId = User.Identity.GetUserId();
            return View();
        }

        public ActionResult AppointmentsHistory()
        {
            return View();
        }
        public ActionResult Messages()
        {
            return View();
        }
    }
}