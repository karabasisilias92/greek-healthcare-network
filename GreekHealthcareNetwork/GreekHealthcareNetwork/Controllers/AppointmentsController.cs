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
        private readonly DoctorsRepository _doctorssRespository = new DoctorsRepository();
        private readonly InsuredsRepository _insuredRespository = new InsuredsRepository();

        // GET: Appointments
        public ActionResult CancelAppointmentDetails(int appointmentId)
        {
            var appointment = new Appointment();
            appointment = _appointmentsRespository.GetAppointmentById(appointmentId);
            return PartialView("_ModalCancelAppointmentPartial", appointment);
        }

        [HttpPost]
        public ActionResult CancelAppointment(int appointmentId)
        {
            //_appointmentsRespository.CancelAppointment(appointmentId);
            // code for sending message
            // Tha allazei ton arithmo twn bookedApointments kata -1???
            return Json("Success");
        }
    }
}