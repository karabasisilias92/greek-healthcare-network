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

        // GET: Profile
        public new ActionResult Profile()
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


            return View(currentUser);
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
    }
}