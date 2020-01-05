using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActionResult BookAppointment(SearchLoginViewModel searchLoginViewModel)
        {
            searchLoginViewModel.MedicalSpecialties = new List<MedicalSpecialty>();
            for (int i = 0; i < Enum.GetNames(typeof(MedicalSpecialty)).Length; i++)
            {
                searchLoginViewModel.MedicalSpecialties.Add((MedicalSpecialty)i);
            }
            return View(searchLoginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = _doctors.GetDoctorById(appointment.DoctorId);
                Insured insured = _insureds.GetInsuredById(appointment.InsuredId);
                byte appointmentDuration = doctor.WorkingHours.SingleOrDefault(d => d.Day == appointment.AppointmentDate.DayOfWeek).AppointmentDuration;
                appointment.AppointmentEndTime = appointment.AppointmentStartTime + new TimeSpan(0, appointmentDuration, 0);
                var appointmentCost = doctor.AppointmentCost.AppointmentCost;
                var insuredPlan = insured.InsuredPlan;
                if (insuredPlan.Name.Equals("Gold"))
                {
                    appointment.InsuredAppointmentCharge = 0;
                    int id = _appointments.AddAppointment(appointment);
                    insured.BookedAppointments++;
                    _insureds.UpdateInsured(insured);
                    return RedirectToAction("SuccessfulBooking", new { id = id });
                }
                else
                {
                    if (insured.BookedAppointments < Convert.ToInt32(insuredPlan.PlanAppoinments))
                    {
                        appointment.InsuredAppointmentCharge = appointmentCost * Convert.ToDecimal(insuredPlan.AppointmentRate / 100);
                    }
                    else
                    {
                        appointment.InsuredAppointmentCharge = appointmentCost * Convert.ToDecimal(insuredPlan.ExceededAppointmentRate / 100);
                    }
                    int id = _appointments.AddAppointment(appointment);
                    insured.BookedAppointments++;
                    _insureds.UpdateInsured(insured);
                    return RedirectToAction("PayAppointmentCharge", "Payments", new { id = id, appointmentCharge = appointment.InsuredAppointmentCharge });
                }
            }
            return View(appointment);
        }

        public ActionResult SuccessfulBooking(int id)
        {
            var appointment = _appointments.GetAppointmentById(id);
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