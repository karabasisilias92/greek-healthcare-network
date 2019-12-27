﻿using System;
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

        // GET: Profile
        public new ActionResult Profile()
        {
            //var userId = User.Identity.GetUserId();
            return View();
        }

        public ActionResult AppointmentsHistory(SearchViewModel searchViewModel)
        {
            searchViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(searchViewModel);
        }

        public ActionResult Messages()
        {
            return View();
        }
    }
}