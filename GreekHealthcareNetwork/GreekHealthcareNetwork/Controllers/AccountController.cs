using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GreekHealthcareNetwork.Models;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using GreekHealthcareNetwork.Repositories;
using System.Drawing;

namespace GreekHealthcareNetwork.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        private readonly InsuredsRepository _insureds = new InsuredsRepository();
        private readonly UsersRepository _users = new UsersRepository();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, SearchLoginViewModel searchLoginViewModel)
        {
            ViewBag.ReturnUrl = returnUrl;
            searchLoginViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchLoginViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            searchLoginViewModel.InsuredPlans = _insureds.GetInsuredPlans().ToList();
            return View(searchLoginViewModel);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(SearchLoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {   
                model.MedicalSpecialties = new List<MedicalSpecialty>();
                for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
                {
                    model.MedicalSpecialties.Add((MedicalSpecialty)i);
                }
                model.InsuredPlans = _insureds.GetInsuredPlans().ToList();
                return View(model);
            }
            // This is in order to enable login both with email and username
            ApplicationUser user;
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                user = db.Users.SingleOrDefault(u => u.Email == model.UserName);
                if (user == null)
                {
                    user = db.Users.SingleOrDefault(u => u.UserName == model.UserName);
                }
            }
            if (user != null)
            {
                model.UserName = user.UserName;
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if(UserManager.IsInRole(user.Id, "Administrator"))
                    {
                        return RedirectToLocal("/Admin/Index");
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    model.MedicalSpecialties = new List<MedicalSpecialty>();
                    for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
                    {
                        model.MedicalSpecialties.Add((MedicalSpecialty)i);
                    }
                    model.InsuredPlans = _insureds.GetInsuredPlans().ToList();
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/DoctorRegister
        [AllowAnonymous]
        public ActionResult DoctorRegister()
        {
            DoctorRegisterViewModel model = new DoctorRegisterViewModel();
            model.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                model.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(model);
        }

        //
        // POST: /Account/DoctorRegister
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoctorRegister(DoctorRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = "defaultUserImage.png";

                if (model.ProfilePicture != null)
                {
                    Image image = Image.FromStream(model.ProfilePicture.InputStream);
                    if (image.Width != image.Height || image.Width < 237.5)
                    {
                        model.MedicalSpecialties = new List<MedicalSpecialty>();
                        for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
                        {
                            model.MedicalSpecialties.Add((MedicalSpecialty)i);
                        }
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
                    Directory.CreateDirectory(Server.MapPath("~/Content/img/Doctors/" + user.Id));
                    string path = Path.Combine(Server.MapPath("~/Content/img/Doctors/" + user.Id), fileName);
                    if (model.ProfilePicture != null)
                    {
                        model.ProfilePicture.SaveAs(path);
                    } 
                    else
                    {
                        string defaultImagePath = Server.MapPath("~/Content/img/defaultUserImage.png");
                        System.IO.File.Copy(defaultImagePath, path, true);
                    }
                    try
                    {
                        int doctorPlanId = _doctors.GetDoctorPlanId(model.MedicalSpecialty);
                        Doctor doctor = new Doctor { UserId = user.Id, OfficeAddress = model.OfficeAddress, MedicalSpecialty = model.MedicalSpecialty, DoctorPlanId = doctorPlanId };
                        //user.ProfilePicture = fileName;
                        //_users.UpdateUser(user);
                        _doctors.InsertDoctor(doctor);
                    }
                    catch (Exception e)
                    {
                        model.MedicalSpecialties = new List<MedicalSpecialty>();
                        for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
                        {
                            model.MedicalSpecialties.Add((MedicalSpecialty)i);
                        }
                        await UserManager.DeleteAsync(user);
                        DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Content/img/Doctors/" + user.Id));
                        EmptyFolder(directory);
                        Directory.Delete(Server.MapPath("~/Content/img/Doctors/" + user.Id));
                        // If we could not create doctor for some reason, something failed, redisplay form

                        ModelState.AddModelError("", "Something went wrong, please try again.");
                        return View(model);
                    }
                    UserManager.AddToRole(user.Id, "Doctor");
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("RegisterDoctorWorkingHours", "Account", new { userId = user.Id });
                }
                AddErrors(result);
            }
            model.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                model.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegisterDoctorWorkingHours(string userId)
        {
            RegisterDoctorWorkingHoursViewModel model = new RegisterDoctorWorkingHoursViewModel();
            model.DoctorId = userId;
            model.Days = new List<DayOfWeek>();
            for (int i = 1; i < Enum.GetNames(typeof(DayOfWeek)).Length; i++)
            {
                model.Days.Add((DayOfWeek)i);
            }
            model.Days.Add((DayOfWeek)0);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterDoctorWorkingHours(RegisterDoctorWorkingHoursViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!CheckIfWorkingHoursEntriesAreValid(model.WorkingHours))
                {
                    model.DoctorId = model.WorkingHours[0].DoctorId;
                    model.Days = new List<DayOfWeek>();
                    for (int j = 1; j < Enum.GetNames(typeof(DayOfWeek)).Length; j++)
                    {
                        model.Days.Add((DayOfWeek)j);
                    }
                    model.Days.Add((DayOfWeek)0);
                    return View(model);
                }
                if (CheckIfWorkingHourEntriesIntecept(model.WorkingHours))
                {
                    model.DoctorId = model.WorkingHours[0].DoctorId;
                    model.Days = new List<DayOfWeek>();
                    for (int j = 1; j < Enum.GetNames(typeof(DayOfWeek)).Length; j++)
                    {
                        model.Days.Add((DayOfWeek)j);
                    }
                    model.Days.Add((DayOfWeek)0);
                    return View(model);
                }
                for(int i = 0; i < model.WorkingHours.Count; i++)
                {
                    var workingHoursEntry = new WorkingHours
                    {
                        Day = model.WorkingHours[i].Day,
                        WorkStartTime = model.WorkingHours[i].WorkStartTime,
                        WorkEndTime = model.WorkingHours[i].WorkEndTime,
                        AppointmentDuration = model.WorkingHours[i].AppointmentDuration,
                        DoctorId = model.WorkingHours[i].DoctorId
                    };
                    try
                    {
                        _doctors.InsertWorkingHoursEntry(workingHoursEntry);
                    }
                    catch (Exception e)
                    {
                        model.DoctorId = model.WorkingHours[i].DoctorId;
                        model.Days = new List<DayOfWeek>();
                        for (int j = 1; j < Enum.GetNames(typeof(DayOfWeek)).Length; j++)
                        {
                            model.Days.Add((DayOfWeek)j);
                        }
                        model.Days.Add((DayOfWeek)0);
                        ModelState.AddModelError("", e.Message);
                        return View(model);
                    }
                }
                return RedirectToAction("DoctorPayDoctorPlan", "Account", new { userId = model.WorkingHours[0].DoctorId });
            }

            model.DoctorId = model.WorkingHours[0].DoctorId;
            model.Days = new List<DayOfWeek>();
            for (int i = 1; i < Enum.GetNames(typeof(DayOfWeek)).Length; i++)
            {
                model.Days.Add((DayOfWeek)i);
            }
            model.Days.Add((DayOfWeek)0);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult DoctorPayDoctorPlan(string userId)
        {
                DoctorPlan doctorPlan = _doctors.GetDoctorPlan(userId);
                PayDoctorPlanViewModel model = new PayDoctorPlanViewModel() { DoctorId = userId, DoctorPlan = doctorPlan };
                return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoctorPayDoctorPlan(PayDoctorPlanViewModel model)
        {
            if (!Request.IsAuthenticated)
            {
                var user = _doctors.GetDoctorById(model.DoctorId).User;
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UserProfile", "User");
            }
        }

        // GET: /Account/ClientRegister
        [AllowAnonymous]
        public ActionResult ClientRegister()
        {
            InsuredRegisterViewModel model = new InsuredRegisterViewModel();
            return View(model);
        }

        //
        // POST: /Account/DoctorRegister
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientRegister(InsuredRegisterViewModel model)
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
                    Directory.CreateDirectory(Server.MapPath("~/Content/img/Insureds/" + user.Id));
                    string path = Path.Combine(Server.MapPath("~/Content/img/Insureds/" + user.Id), fileName);
                    if (model.ProfilePicture != null)
                    {
                        model.ProfilePicture.SaveAs(path);
                    }
                    else
                    {
                        string defaultImagePath = Server.MapPath("~/Content/img/defaultUserImage.png");
                        System.IO.File.Copy(defaultImagePath, path, true);
                    }
                    try
                    {
                        Insured insured = new Insured { UserId = user.Id, HomeAddress = model.HomeAddress};
                        //user.ProfilePicture = fileName;
                        //_users.UpdateUser(user);
                        _insureds.InsertInsured(insured);
                    }
                    catch (Exception)
                    {
                        await UserManager.DeleteAsync(user);
                        DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Content/img/Doctors/" + user.Id));
                        EmptyFolder(directory);
                        Directory.Delete(Server.MapPath("~/Content/img/Doctors/" + user.Id));
                        // If we could not create doctor for some reason, something failed, redisplay form

                        ModelState.AddModelError("", "Something went wrong, please try again.");
                        return View(model);
                    }
                    UserManager.AddToRole(user.Id, "Insured");
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("ClientPayInsuredPlan", "Account", new { userId = user.Id });
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ClientPayInsuredPlan(string userId)
        {
            PayInsuredPlanViewModel model = new PayInsuredPlanViewModel();
            model.InsuredId = userId;
            model.InsuredPlans = _insureds.GetInsuredPlans().ToList();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientPayInsuredPlan(PayInsuredPlanViewModel model)
        {
            if (!Request.IsAuthenticated)
            {
                var user = _insureds.GetInsuredById(model.InsuredId).User;
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UserProfile", "User");
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult LogOff(int? i)
        {
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void EmptyFolder(DirectoryInfo directory)
        {

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                EmptyFolder(subdirectory);
                subdirectory.Delete();
            }

        }


        public bool CheckIfWorkingHourEntriesIntecept(List<WorkingHours> workingHoursList)
        {
            bool result = false;
            for (int i = 0; i < workingHoursList.Count; i++)
            {
                for (int j = i + 1; j < workingHoursList.Count; j++)
                {
                    if (workingHoursList[i].Day == workingHoursList[j].Day && ((workingHoursList[j].WorkStartTime <= workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime > workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime <= workingHoursList[i].WorkEndTime) ||
                                                                               (workingHoursList[j].WorkStartTime <= workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime > workingHoursList[i].WorkEndTime) ||
                                                                               (workingHoursList[j].WorkStartTime > workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime <= workingHoursList[i].WorkEndTime) ||
                                                                               (workingHoursList[j].WorkStartTime > workingHoursList[i].WorkStartTime && workingHoursList[j].WorkStartTime < workingHoursList[i].WorkEndTime && workingHoursList[j].WorkEndTime > workingHoursList[i].WorkEndTime)))
                    {
                        result = true;
                        ModelState.AddModelError("", "Entry " + (i + 1) + " is identical or intercepts with entry " + (j + 1) + "! Please check and fix.");
                    }
                }
            }

            return result;
        }

        public bool CheckIfWorkingHoursEntriesAreValid(List<WorkingHours> workingHourList)
        {
            bool result = true;
            double minutes;
            for (int i = 0; i < workingHourList.Count; i++)
            {
                minutes = (workingHourList[i].WorkEndTime - workingHourList[i].WorkStartTime).Value.TotalMinutes;
                if (minutes <= 0 || minutes % workingHourList[i].AppointmentDuration != 0)
                {
                    result = false;
                    ModelState.AddModelError("", "Entry " + (i + 1) + ": Work End Time must be greater than Work Start Time and their difference in minutes must be divided exactly by Appointment Duration");
                }
            }
            return result;
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}