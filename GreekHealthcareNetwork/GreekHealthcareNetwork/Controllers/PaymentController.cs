using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace GreekHealthcareNetwork.Controllers
{
    public class PaymentController : Controller
    {
        private Payment payment;

        // GET: Payment
        public ActionResult Pay()
        {
            // Get a reference to the config
            var config = ConfigManager.Instance.GetProperties();

            // Use OAuthTokenCredential to request an access token from PayPal
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            return View();
        }


        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "Euro",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your generated invoice number", //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

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
                var insured = _insureds.GetInsuredById(userId);
                if (insured.User.SubscriptionEndDate >= DateTime.Now)
                {
                    planFee = _insureds.GetInsuredPlan(planId).PlanFee - insured.InsuredPlan.PlanFee;
                }
                else
                {
                    planFee = _insureds.GetInsuredPlan(planId).PlanFee;
                }
            }
            PaySubcriptionViewModel model = new PaySubcriptionViewModel() { UserId = userId, PlanId = planId, PlanFee = planFee };
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
                    if (!user.IsActive)
                    {
                        _users.UpdateSubscriptionEndDate(user.Id);
                        _users.ActivateUser(user.Id);
                    }
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