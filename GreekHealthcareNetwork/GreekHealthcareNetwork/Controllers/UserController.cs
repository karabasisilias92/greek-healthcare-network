using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using Microsoft.AspNet.Identity;

namespace GreekHealthcareNetwork.Controllers
{
    public class UserController : Controller
    {
        private readonly AppointmentsRespository _appointmentsRespository = new AppointmentsRespository();
        private readonly UsersRepository _usersRepository = new UsersRepository();
        private readonly DoctorsRepository _doctorsRepository = new DoctorsRepository();
        private readonly InsuredsRepository _insuredsRepository = new InsuredsRepository();

        private ProfileDetailsViewModel GetCurrentUser()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = new ProfileDetailsViewModel();

            currentUser.User = _usersRepository.GetUserById(userId);

            if (User.IsInRole("Doctor"))
            {
                currentUser.Doctor = _doctorsRepository.GetDoctorById(userId);
            }

            if (User.IsInRole("Insured"))
            {
                currentUser.Insured = _insuredsRepository.GetInsuredById(userId);
            }

            return currentUser;
        }

        // GET: Profile
        public ActionResult UserProfile()
        {

            return View(GetCurrentUser());
        }

        public ActionResult AppointmentsHistory(SearchLoginViewModel searchLoginViewModel)
        {
            searchLoginViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchLoginViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(searchLoginViewModel);
        }

        public ActionResult Messages()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            return View(GetCurrentUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ProfileDetailsViewModel currentUser)
        {
            if (!ModelState.IsValid)
            {
                var user = GetCurrentUser();
                currentUser.Insured.InsuredPlan = user.Insured.InsuredPlan;
                currentUser.User.SubscriptionEndDate = user.User.SubscriptionEndDate;
                currentUser.User.ProfilePicture = user.User.ProfilePicture;
                return View(currentUser);
            }

            try
            {
                var user = GetCurrentUser();
                currentUser.Insured.InsuredPlan = user.Insured.InsuredPlan;
                currentUser.User.SubscriptionEndDate = user.User.SubscriptionEndDate;
                currentUser.User.ProfilePicture = user.User.ProfilePicture;
                _usersRepository.UpdateUser(currentUser);
            }
            catch (Exception error)
            {
                ModelState.AddModelError("", error);
                var user = GetCurrentUser();
                currentUser.Insured.InsuredPlan = user.Insured.InsuredPlan;
                currentUser.User.SubscriptionEndDate = user.User.SubscriptionEndDate;
                currentUser.User.ProfilePicture = user.User.ProfilePicture;
                return View(currentUser);
            }
            

            return RedirectToAction("UserProfile");
        }
    }
}