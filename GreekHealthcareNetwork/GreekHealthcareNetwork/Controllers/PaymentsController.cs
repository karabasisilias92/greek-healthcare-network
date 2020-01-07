using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using Microsoft.AspNet.Identity;
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
        private ApplicationUserManager _userManager;
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        private readonly InsuredsRepository _insureds = new InsuredsRepository();
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

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Payments
        [AllowAnonymous]
        public ActionResult PaySubscription(string userId, int planId)
        {
            decimal planFee;
            if (UserManager.IsInRole(userId, "Doctor"))
            {
                planFee = _doctors.GetDoctorPlan(userId).Fee;
            }
            else
            {
                planFee = _insureds.GetInsuredPlan(planId).PlanFee;
            }
            PaySubcriptionViewModel model = new PaySubcriptionViewModel() { UserId = userId, PlanId = planId, PlanFee = planFee};
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
                if (UserManager.IsInRole(model.UserId, "Doctor"))
                {
                    var doctor = _doctors.GetDoctorById(model.UserId);
                    if (doctor.WorkingHours != null && doctor.WorkingHours.Count > 0)
                    {
                        _users.ActivateUser(doctor.UserId);
                    }
                    _users.UpdateSubscriptionEndDate(doctor.UserId);
                }
                else if (UserManager.IsInRole(model.UserId, "Insured"))
                {
                    _insureds.UpdateInsuredPlan(user.Id, model.PlanId);
                    _users.ActivateUser(user.Id);
                    _users.UpdateSubscriptionEndDate(user.Id);
                }
            }
            if (!Request.IsAuthenticated)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UserProfile", "User");
            }
        }

        [Authorize(Roles = "Insured")]
        public ActionResult PayAppointmentCharge(int id, decimal appointmentCharge)
        {           
            PayAppointmentChargeViewModel model = new PayAppointmentChargeViewModel() { AppointmentId = id, AppointmentCharge = appointmentCharge };
            return View(model);
        }

        [Authorize(Roles = "Insured")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayAppointmentCharge(PayAppointmentChargeViewModel model)
        {
            return RedirectToAction("SuccessfulBooking", "Insureds", new { id = model.AppointmentId });
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