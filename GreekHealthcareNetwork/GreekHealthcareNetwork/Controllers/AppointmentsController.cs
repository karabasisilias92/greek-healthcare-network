using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentsRespository _appointmentsRespository = new AppointmentsRespository();

        // GET: Appointments
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CancelAppointment(int appointmentId)
        {
            _appointmentsRespository.CancelAppointment(appointmentId);
            // code for sending message
            return Json("Success");
        }
    }
}