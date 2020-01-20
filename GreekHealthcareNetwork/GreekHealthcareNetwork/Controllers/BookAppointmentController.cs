using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Models.Enums;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Insured")]
    public class BookAppointmentController : ApiController
    {
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        private readonly InsuredsRepository _insureds = new InsuredsRepository();
        private readonly AppointmentsRespository _appointments = new AppointmentsRespository();

        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public IHttpActionResult BookAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = _doctors.GetDoctorById(appointment.DoctorId);
                Insured insured = _insureds.GetInsuredById(appointment.InsuredId);
                byte appointmentDuration = doctor.WorkingHours.SingleOrDefault(d => d.Day == appointment.AppointmentDate.DayOfWeek && d.WorkStartTime <= appointment.AppointmentStartTime && d.WorkEndTime >= appointment.AppointmentStartTime).AppointmentDuration;
                appointment.AppointmentEndTime = appointment.AppointmentStartTime + new TimeSpan(0, appointmentDuration, 0);
                appointment.AppointmentStatus = AppointmentStatus.Upcoming;
                appointment.PaidByCompany = false;
                var appointmentCost = doctor.DoctorPlan.AppointmentCost;
                var insuredPlan = insured.InsuredPlan;
                int id = _appointments.AppointmentEntryExists(insured.UserId, doctor.UserId, appointment.AppointmentDate, appointment.AppointmentStartTime);
                if (insuredPlan.Name.Equals("Gold"))
                {
                    appointment.InsuredAppointmentCharge = 0;
                    appointment.AppointmentChargePaid = true;
                    try
                    {
                        if (id == 0)
                        {
                            id = _appointments.AddAppointment(appointment);
                        }
                        else
                        {
                            appointment.Id = id;
                            _appointments.UpdateAppointment(appointment);
                        }
                        insured.BookedAppointments++;
                        _insureds.UpdateInsured(insured);
                        return Ok(new { action = "SuccessfulBooking", controller = "Insureds", id = id, appointmentCharge = 0 });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", $"We are sorry but another user has just booked the appointment that starts at {appointment.AppointmentStartTime.ToString(@"hh\:mm")}. Please choose another time slot or find another doctor with the same time slot available.");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    appointment.AppointmentChargePaid = false;
                    if (insured.BookedAppointments < Convert.ToInt32(insuredPlan.PlanAppoinments))
                    {
                        appointment.InsuredAppointmentCharge = appointmentCost * Convert.ToDecimal(insuredPlan.AppointmentRate / 100);
                    }
                    else
                    {
                        appointment.InsuredAppointmentCharge = appointmentCost * Convert.ToDecimal(insuredPlan.ExceededAppointmentRate / 100);
                    }
                    try
                    {
                        if (id == 0)
                        {
                            id = _appointments.AddAppointment(appointment);
                        }
                        else
                        {
                            appointment.Id = id;
                            _appointments.UpdateAppointment(appointment);
                        }
                        insured.BookedAppointments++;
                        _insureds.UpdateInsured(insured);
                        return Ok(new { action = "PayAppointmentCharge", controller = "Payment", id = id, appointmentCharge = appointment.InsuredAppointmentCharge });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", $"We are sorry but another user has just booked the appointment that starts at {appointment.AppointmentStartTime.ToString(@"hh\:mm")}. Please choose another time slot or find another doctor with the same time slot available.");
                        return BadRequest(ModelState);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpPut, System.Web.Http.HttpPatch]
        [ValidateAntiForgeryToken]
        public IHttpActionResult EditAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Doctor doctor = _doctors.GetDoctorById(appointment.DoctorId);
            byte appointmentDuration = doctor.WorkingHours.SingleOrDefault(d => d.Day == appointment.AppointmentDate.DayOfWeek && d.WorkStartTime <= appointment.AppointmentStartTime && d.WorkEndTime >= appointment.AppointmentStartTime).AppointmentDuration;
            appointment.AppointmentEndTime = appointment.AppointmentStartTime + new TimeSpan(0, appointmentDuration, 0);
            appointment.AppointmentStatus = AppointmentStatus.Upcoming;
            appointment.AppointmentChargePaid = true;
            _appointments.UpdateAppointment(appointment);

            return Ok();
        }
    }
}
