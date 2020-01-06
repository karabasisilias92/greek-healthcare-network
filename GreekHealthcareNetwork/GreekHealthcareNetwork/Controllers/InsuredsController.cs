using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
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
            _appointments.DeleteAppointment(appointmentId);
            var insured = _insureds.GetInsuredById(insuredId);
            insured.BookedAppointments--;
            _insureds.UpdateInsured(insured);
            return View(appointment);
        }

        public ActionResult CancelledBookingDueToNotPayingOnTime(int appointmentId, string insuredId)
        {
            var appointment = _appointments.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return RedirectToAction("BookAppointment");
            }
            _appointments.DeleteAppointment(appointmentId);
            var insured = _insureds.GetInsuredById(insuredId);
            insured.BookedAppointments--;
            _insureds.UpdateInsured(insured);
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
    }
}