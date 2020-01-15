﻿using GreekHealthcareNetwork.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;

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
        // GET: Admin
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = UserManager.Users.SingleOrDefault(u => u.Id == userId);
            ViewBag.FullName = user.FirstName + " " + user.LastName;
            return View();
        }
        public ActionResult Doctors()
        {
            return View();
        }
        //
        public ActionResult UpdateDoctors()
        {
            return View();
        }
        public ActionResult Insureds()
        {
            return View();
        }
    }
}