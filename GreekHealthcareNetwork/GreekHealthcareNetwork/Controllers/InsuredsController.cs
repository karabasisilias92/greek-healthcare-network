using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Models.Enums;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Insured, Administrator")]
    public class InsuredsController : Controller
    {
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        private readonly InsuredsRepository _insureds = new InsuredsRepository();
        private readonly AppointmentsRespository _appointments = new AppointmentsRespository();
        // GET: Insureds
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BookAppointment()
        {
            SearchLoginViewModel searchLoginViewModel = new SearchLoginViewModel();
            searchLoginViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchLoginViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(searchLoginViewModel);
        }

        public ActionResult SuccessfulBooking(int id)
        {
            var appointment = _appointments.GetAppointmentById(id);
            return View(appointment);
        }

        public ActionResult CancelledBooking(int appointmentId, string insuredId)
        {
            var appointment = _appointments.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return RedirectToAction("BookAppointment");
            }
            appointment.AppointmentChargePaid = true; // in order not to be deleted by db stored procedure
            appointment.AppointmentStatus = AppointmentStatus.Canceled;
            _appointments.UpdateAppointment(appointment);
            var insured = _insureds.GetInsuredById(insuredId);
            insured.BookedAppointments--;
            _insureds.UpdateInsured(insured);
            Session.Remove("appointmentId");
            Session.Remove("paymentFor");
            Session.Remove("insuredId");
            Session.Remove("paymentItemName");
            Session.Remove("Transaction description");
            Session.Remove("price");
            return View(appointment);
        }

        public ActionResult CancelledBookingDueToNotPayingOnTime(int appointmentId, string insuredId)
        {
            var appointment = (Appointment)Session["appointment" + appointmentId];
            var insured = _insureds.GetInsuredById(insuredId);
            insured.BookedAppointments--;
            _insureds.UpdateInsured(insured);
            Session.Remove("timestamp" + appointmentId);
            Session.Remove("paymentFor");
            Session.Remove("insuredId");
            Session.Remove("paymentItemName");
            Session.Remove("Transaction description");
            Session.Remove("price" + appointmentId);
            return View(appointment);
        }

        public ActionResult Doctors(SearchLoginViewModel searchLoginViewModel)
        {
            searchLoginViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchLoginViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(searchLoginViewModel);
        }

        [System.Web.Mvc.HttpPut]
        public ActionResult SetRefundToZero(string insuredId)
        {
            var insured = _insureds.GetInsuredById(insuredId);
            insured.RefundPending = 0;
            try
            {
                _insureds.UpdateInsured(insured);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(400);
            }            
        }
    }
}