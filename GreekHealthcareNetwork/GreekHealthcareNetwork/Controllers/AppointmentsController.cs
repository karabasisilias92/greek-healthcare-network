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
        private readonly MessagesRepository _messagesRepository = new MessagesRepository();

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
            SearchLoginViewModel model = new SearchLoginViewModel();
            var appointment = _appointmentsRespository.GetAppointmentById(appointmentId);

            // Sets appointmentStatus to canceled and decrease the booked appointments by 1
            try
            {
                _appointmentsRespository.CancelAppointment(appointmentId);
            }
            catch (Exception)
            {
                return Json("Error");
            }

            // Send message to the other part that the appointment canceled
            model.Message = new Message();
            model.Message.ConversationId = _messagesRepository.NewConversationId() + 1;
            if (User.IsInRole("Insured"))
            {
                model.Message.SenderId = appointment.Insured.User.Id;
                model.Message.RecipientId = appointment.Doctor.User.Id;
                model.Message.MessageText = "[AUTOMATED MESSAGE] \n " +
                                            "Client cancelled the appointment on " + appointment.AppointmentDate.Date.ToString("dd-MM-yyyy") + " at " + appointment.AppointmentStartTime + "-" + appointment.AppointmentEndTime + "!";
            }
            if (User.IsInRole("Doctor"))
            {
                model.Message.SenderId = appointment.Doctor.User.Id;
                model.Message.RecipientId = appointment.Insured.User.Id;
                model.Message.MessageText = "[AUTOMATED MESSAGE] \n" +
                                            "Doctor cancelled the appointment on " + appointment.AppointmentDate.Date.ToString("dd-MM-yyyy") + " at " + appointment.AppointmentStartTime + "-" + appointment.AppointmentEndTime + "!";
            }
            model.Message.Subject = "Appointment Cancellation";
            model.Message.SentDate = DateTime.Now.Date;
            model.Message.SentTime = DateTime.Now.TimeOfDay;
            model.Message.MessageStatus = MessageStatus.Unread;
            model.FirstName = "";
            model.LastName = "";
            model.InsuredPlans = new List<InsuredPlan>();
            model.DoctorSpecialty = -1;
            model.MedicalSpecialties = new List<MedicalSpecialty>();
            model.AppointmentDate = null;
            model.UserName = User.Identity.Name;
            model.Password = "!Doctor123";
            model.RememberMe = false;
            model.VisitorMessage = null;

            try
            {
                _messagesRepository.Add(model.Message);
            }
            catch (Exception)
            {
                return Json("Error");
            }

            return Json("Success");
        }
    }
}