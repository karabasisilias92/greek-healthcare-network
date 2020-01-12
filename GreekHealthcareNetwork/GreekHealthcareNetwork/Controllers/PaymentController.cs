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
        private Payment payment;

        // GET: Payment
        public ActionResult PayWithPayPal()
        {
            // Get context from the paypal based on clientId and clientSecret for payment
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

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
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PayWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(10000000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
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
                        return View("PaymentFailed");
                    }
                }
            }
            catch (Exception)
            {
                return View("PaymentFailed");
            }

            string paymentFor = (string)Session["paymentFor"];
            if (paymentFor.Equals("Subscription"))
            {
                string userId = (string)Session["userId"];
                var user = _users.GetUserById(userId);
                using (var db = new ApplicationDbContext())
                {
                    if (UserManager.IsInRole(userId, "Doctor"))
                    {
                        var doctor = _doctors.GetDoctorById(userId);
                        if (doctor.WorkingHours != null && doctor.WorkingHours.Count > 0)
                        {
                            _users.ActivateUser(doctor.UserId);
                        }
                        _users.UpdateSubscriptionEndDate(doctor.UserId);
                    }
                    else if (UserManager.IsInRole(userId, "Insured"))
                    {
                        int planId = (int)Session["planId"];
                        _insureds.UpdateInsuredPlan(user.Id, planId);
                        if (!user.IsActive)
                        {
                            _users.UpdateSubscriptionEndDate(user.Id);
                            _users.ActivateUser(user.Id);
                        }
                    }
                }
                if (!Request.IsAuthenticated)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("UserProfile", "User");
                }
            }
            else
            {
                int appointmentId = (int)Session["appointmentId"];
                return RedirectToAction("SuccessfulBooking", "Insureds", new { id = appointmentId });
            }
            
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
            decimal price = Math.Floor(0.85m * Convert.ToDecimal(Session["price"]) * 100) / 100;
            string priceString = price.ToString("0.00");
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = (string)Session["paymentItemName"],
                currency = "EUR",
                price = priceString,
                quantity = "1",
                sku = "N/A"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object
            string paymentFor = (string)Session["paymentFor"];
            string cancelUrl;
            if (paymentFor.Equals("Subscription"))
            {
                cancelUrl = "https://localhost:44310/Payment/PaySubscriptionCancelled";
            }
            else
            {
                int appointmentId = (int)Session["appointmentId"];
                string insuredId = (string)Session["insuredId"];
                cancelUrl = "https://localhost:44310/Insureds/CancelledBooking?appointmentId=" + appointmentId + "&insuredId=" + insuredId;
            }
            var redirUrls = new RedirectUrls()
            { 
                cancel_url = cancelUrl,
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = (0.15m * Convert.ToDecimal(Session["price"])).ToString("0.00"),
                shipping = "0",
                subtotal = priceString
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "EUR",
                total = (Convert.ToDecimal(details.tax) + Convert.ToDecimal(details.shipping) + Convert.ToDecimal(details.subtotal)).ToString("0.00"), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = (string)Session["Transaction description"],
                invoice_number = Convert.ToString((new Random()).Next(10000000)), //Generate an Invoice No  
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

        [AllowAnonymous]
        public ActionResult PayInsuredSubscription(string userId, int planId)
        {
            decimal planFee;            
            var insured = _insureds.GetInsuredById(userId);
            if (insured.User.SubscriptionEndDate >= DateTime.Now)
            {
                planFee = _insureds.GetInsuredPlan(planId).PlanFee - insured.InsuredPlan.PlanFee;
            }
            else
            {
                planFee = _insureds.GetInsuredPlan(planId).PlanFee;
            }
            PaySubcriptionViewModel model = new PaySubcriptionViewModel() { UserId = userId, PlanId = planId, PlanFee = planFee };
            return View(model);
        }        

        [Authorize(Roles = "Insured")]
        public ActionResult PayAppointmentCharge(int id, decimal appointmentCharge)
        {
            PayAppointmentChargeViewModel model = new PayAppointmentChargeViewModel() { AppointmentId = id, AppointmentCharge = appointmentCharge };
            return View(model);
        }

        public async Task<ActionResult> PaymentFailed()
        {
            string userId = (string)Session["userId"];
            if (userId == null)
            {
                userId = (string)Session["insuredId"];
            }
            ViewBag.UserId = userId;
            if (!Request.IsAuthenticated)
            {
                var user = _users.GetUserById(userId);
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("PaymentFailed");
            }
            return View();
        }

        public async Task<ActionResult> PaySubscriptionCancelled()
        {
            string userId = (string)Session["userId"];
            ViewBag.UserId = userId;
            if (!Request.IsAuthenticated)
            {
                var user = _users.GetUserById(userId);
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("PaySubscriptionCancelled");
            } 
            return View();
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