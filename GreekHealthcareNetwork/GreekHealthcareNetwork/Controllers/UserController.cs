using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        private ProfileDetailsViewModel UpdatedUser(ProfileDetailsViewModel modifiedUser)
        {
            var updatedUser = GetCurrentUser();
            updatedUser.User.FirstName = modifiedUser.User.FirstName;
            updatedUser.User.LastName = modifiedUser.User.LastName;
            updatedUser.User.DoB = modifiedUser.User.DoB;
            updatedUser.User.AMKA = modifiedUser.User.AMKA;
            updatedUser.User.PaypalAccount = modifiedUser.User.PaypalAccount;
            updatedUser.User.Email = modifiedUser.User.Email;
            updatedUser.User.PhoneNumber = modifiedUser.User.PhoneNumber;

            if (HttpContext.User.IsInRole("Doctor"))
            {
                updatedUser.Doctor.MedicalSpecialty = modifiedUser.Doctor.MedicalSpecialty;
                if(modifiedUser.Doctor.WorkingHours != null)
                {
                    updatedUser.Doctor.WorkingHours = modifiedUser.Doctor.WorkingHours;
                }
                updatedUser.Doctor.OfficeAddress = modifiedUser.Doctor.OfficeAddress;
            }
            if (HttpContext.User.IsInRole("Insured"))
            {
                updatedUser.Insured.HomeAddress = modifiedUser.Insured.HomeAddress;
            }

            return updatedUser;
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
        public ActionResult EditProfile(ProfileDetailsViewModel modifiedUser)
        {

             var user = UpdatedUser(modifiedUser);
            string userId = user.User.Id;

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            string path = "";
            string fileName = user.User.ProfilePicture;
            string fileNameExt = Path.GetExtension(fileName);

            if (modifiedUser.ProfilePicture != null)
            {
                Image image = Image.FromStream(modifiedUser.ProfilePicture.InputStream);
                if (image.Width != image.Height || image.Width < 237.5)
                {
                    ModelState.AddModelError("", "The profile picture width must be equal to its height and the width must be also over 237.5 pixels");
                    return View(user);
                }
                try
                {
                    fileNameExt = Path.GetExtension(modifiedUser.ProfilePicture.FileName);
                    if (HttpContext.User.IsInRole("Insured"))
                    {
                        path = Path.Combine(Server.MapPath("~/Content/img/Insureds/" + userId + "/"), userId + fileNameExt);
                    }
                    if (HttpContext.User.IsInRole("Doctor"))
                    {
                        path = Path.Combine(Server.MapPath("~/Content/img/Doctors/" + userId + "/"), userId + fileNameExt);
                    }
                    modifiedUser.ProfilePicture.SaveAs(path);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex);
                }
            }

            user.User.ProfilePicture = Path.GetFileName(path);

            try
            {
                _usersRepository.UpdateUser(user);
            }
            catch (Exception error)
            {
                ModelState.AddModelError("", error.Message);
                return View(user);
            }

            return RedirectToAction("UserProfile");
        }
    }
}