using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly UsersRepository _usersRepository = new UsersRepository();
        private readonly DoctorsRepository _doctorsRepository = new DoctorsRepository();
        private PlansRepository _plans = new PlansRepository();

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

        private ProfileDetailsViewModel AdminUpdatedUser(ProfileDetailsViewModel modifiedUser)
        {
            var adminUpdatedUser = new ProfileDetailsViewModel();
            if (HttpContext.User.IsInRole("Administrator"))
            {
                var userId = modifiedUser.User.Id;
                adminUpdatedUser.User = _usersRepository.GetUserById(userId);

                adminUpdatedUser.User.FirstName = modifiedUser.User.FirstName;
                adminUpdatedUser.User.LastName = modifiedUser.User.LastName;
                adminUpdatedUser.User.AMKA = modifiedUser.User.AMKA;
                adminUpdatedUser.User.PaypalAccount = modifiedUser.User.PaypalAccount;
                adminUpdatedUser.User.PhoneNumber = modifiedUser.User.PhoneNumber;

                var userRole = _usersRepository.GetRoleNameById(adminUpdatedUser.User.Roles.ElementAt(0).RoleId);

                if (userRole == "Doctor")
                {
                    adminUpdatedUser.Doctor = _doctorsRepository.GetDoctorById(userId);
                    adminUpdatedUser.Doctor.MedicalSpecialty = modifiedUser.Doctor.MedicalSpecialty;
                    adminUpdatedUser.Doctor.OfficeAddress = modifiedUser.Doctor.OfficeAddress;
                }
                else if (userRole == "Insured")
                {

                }
                else
                {
                    throw new ArgumentException();
                }

            }
            else
            {
                throw new ArgumentException();
            }
            return adminUpdatedUser;
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
        public ActionResult Insureds()
        {
            return View();
        }

        public ActionResult VisitorMessages()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AdminWithLessVisitorMessagesUnreplied()
        {
            var messages = new MessagesRepository();
            var adminId = messages.AdminWithLessVisitorMessagesUnreplied();
            return Json(new { adminId = adminId}, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult AdminWithLessNonVisitorMessagesUnreplied()
        {
            var messages = new MessagesRepository();
            var adminId = messages.AdminWithLessNonVisitorMessagesUnreplied();
            return Json(new { adminId = adminId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoctorPlans()
        {
            DoctorPlansViewModel model = new DoctorPlansViewModel();
            model.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                model.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            model.DoctorPlans = _plans.GetDoctorPlans();
            return View(model);
        }

        public ActionResult CreateAdminUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdminUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = "defaultUserImage.png";

                if (model.ProfilePicture != null)
                {
                    Image image = Image.FromStream(model.ProfilePicture.InputStream);
                    if (image.Width != image.Height || image.Width < 237.5)
                    {
                        ModelState.AddModelError("", "The profile picture width must be equal to its height and the width must be also over 237.5 pixels");
                        return View(model);
                    }
                    fileName = model.ProfilePicture.FileName;
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    DoB = model.DoB,
                    AMKA = model.AMKA,
                    PaypalAccount = model.PaypalAccount,
                    ProfilePicture = fileName
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user = UserManager.FindByName(model.UserName);
                    Directory.CreateDirectory(Server.MapPath("~/Content/img/Admins/" + user.Id));
                    string path = Path.Combine(Server.MapPath("~/Content/img/Admins/" + user.Id), fileName);
                    if (model.ProfilePicture != null)
                    {
                        model.ProfilePicture.SaveAs(path);
                    }
                    else
                    {
                        string defaultImagePath = Server.MapPath("~/Content/img/defaultUserImage.png");
                        System.IO.File.Copy(defaultImagePath, path, true);
                    }
                    UserManager.AddToRole(user.Id, "Administrator");
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("AdminUserCreatedSuccessfully", "Admin");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AdminUserCreatedSuccessfully()
        {
            return View();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(ProfileDetailsViewModel modifiedUser)
        {
            var adminUpdatedUser = new ProfileDetailsViewModel();
            if (!ModelState.IsValid)
            {
                //return Json(new { success = false, responseText = "Something went wrong! Please try again." }, JsonRequestBehavior.AllowGet);
                return new HttpStatusCodeResult(400, "Something went wrong! Please try again.");
            }

            try
            {
                adminUpdatedUser = AdminUpdatedUser(modifiedUser);
            }
            catch (Exception)
            {
                //return Json(new { success = false, responseText = "Something went wrong! Please try again." }, JsonRequestBehavior.AllowGet);
                return new HttpStatusCodeResult(400, "Something went wrong! Please try again.");
            }

            try
            {
                _usersRepository.UpdateUser(adminUpdatedUser);
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(400, "Something went wrong! Please try again.");
            }

            return Json(new { success = true, responseText = "Updated Succesfully!" });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}