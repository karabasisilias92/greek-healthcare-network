using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    public class PaymentsController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        private readonly UsersRepository _users = new UsersRepository();
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        // GET: Payments
        [AllowAnonymous]
        public ActionResult PaySubscription(string userId)
        {
            PaySubcriptionViewModel model = new PaySubcriptionViewModel() { UserId = userId };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PaySubscription(PaySubcriptionViewModel model)
        {
            var user = _users.GetUserById(model.UserId);
            using (var db = new ApplicationDbContext())
            {
                string doctorRoleId = _users.GetRoleIdByName("Doctor");
                string insuredRoleId = _users.GetRoleIdByName("Insured");
                if (user.Roles.Any(r => r.RoleId.Equals(doctorRoleId)))
                {
                    var doctor = _doctors.GetDoctorById(model.UserId);
                    if (doctor.WorkingHours != null && doctor.WorkingHours.Count > 0)
                    {
                        _users.ActivateUser(doctor.UserId);
                    }
                    _users.UpdateSubscriptionEndDate(doctor.UserId);
                }
                else if (user.Roles.Any(r => r.RoleId.Equals(insuredRoleId)))
                {
                    _users.ActivateUser(user.Id);
                    _users.UpdateSubscriptionEndDate(user.Id);
                }
            }            
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PayDoctor()
        {
            return View();
        }

        public ActionResult RefundPatient()
        {
            return View();
        }
    }
}