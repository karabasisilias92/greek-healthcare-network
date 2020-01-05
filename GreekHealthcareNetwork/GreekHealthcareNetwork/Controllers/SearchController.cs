using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GreekHealthcareNetwork.Controllers
{
    public class SearchController : ApiController
    {
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        private readonly AppointmentsRespository _appointments = new AppointmentsRespository();

        [HttpGet]
        [Route("api/Search/SearchDoctorResults")]
        public IHttpActionResult SearchResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty)
        {
            var doctors = _doctors.GetFilteredDoctors(doctorsFirstName, doctorsLastName, doctorsSpecialty);
            if (doctors == null)
            {
                return NotFound();
            }
            return Ok(doctors);
        }

        [HttpGet]
        [Route("api/Search/SearchDoctorById/{doctorId}")]
        public IHttpActionResult SearchDoctorById(string doctorId)
        {
            var doctor = _doctors.GetDoctorById(doctorId);
            if(doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpGet]
        [Route("api/Search/AppointmentsSearchResults")]
        public IHttpActionResult SearchResults(string firstName, string lastName, int doctorsSpecialty, DateTime appointmentDay, string userId)
        {
            var appointments = _appointments.GetFilteredAppointments(firstName, lastName, doctorsSpecialty, appointmentDay, userId);
            if (appointments == null)
            {
                return NotFound();
            }
            return Ok(appointments);
        }

        //[HttpGet]
        //[Route("api/Search/InsuredAppointmentsSearchResults")]
        //public IHttpActionResult SearchResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty, DateTime appointmentDay, string userId)
        //{
        //    var appointments = _appointments.GetDoctorFilteredAppointments(doctorsFirstName, doctorsLastName, doctorsSpecialty, appointmentDay, userId);
        //    if (appointments == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(appointments);
        //}

        //[HttpGet]
        //[Route("api/Search/DoctorAppointmentsSearchResults")]
        //public IHttpActionResult searchResults(string insuredsFirstName, string insuredsLastName, DateTime appointmentDate, string userId)
        //{
        //    var appointments = _appointments.GetInsuredFilteredAppointments(insuredsFirstName, insuredsLastName, appointmentDate, userId);
        //    if (appointments == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(appointments);
        //}

        [HttpGet]
        [Route("api/Search/SearchAppointmentById/{appointmentId}")]
        public IHttpActionResult SearchAppointmentById(int appointmentId)
        {
            var appointment = _appointments.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpGet]
        [Route("api/Search/GetAvailableAppointmentSlots")]
        public IHttpActionResult GetAvailableAppointmentSlots(DateTime appointmentDay, string doctorId)
        {
            var doctor = _doctors.GetDoctorById(doctorId);
            var workingHoursOfDay = doctor.WorkingHours.Where(w => w.Day == appointmentDay.DayOfWeek).ToList();
            if (workingHoursOfDay == null || workingHoursOfDay.Count() == 0)
            {
                return NotFound();
            }
            var appointmentSlots = new List<Appointment>();
            var appointments = _appointments.GetDoctorAppointmentsOnDate(appointmentDay, doctorId);
            var appointmentsStartTime = appointments.Select(app => app.AppointmentStartTime).ToList();
            if (appointments == null || appointments.Count() == 0)
            {
                foreach (var item in workingHoursOfDay)
                {
                    var startTime = item.WorkStartTime;
                    var endTime = item.WorkEndTime;
                    var appDuration = item.AppointmentDuration;
                    while (startTime < endTime)
                    {
                        var appointment = new Appointment();
                        appointment.Doctor = null;
                        appointment.Insured = null;
                        appointment.AppointmentDate = appointmentDay;
                        appointment.AppointmentStartTime = (TimeSpan)startTime;
                        appointment.AppointmentEndTime = (TimeSpan)startTime + new TimeSpan(0, appDuration, 0);
                        appointmentSlots.Add(appointment);
                        startTime = appointment.AppointmentEndTime;
                    }
                }
            }
            else
            {
                foreach (var item in workingHoursOfDay)
                {
                    var startTime = item.WorkStartTime;
                    var endTime = item.WorkEndTime;
                    var appDuration = item.AppointmentDuration;
                    while (startTime < endTime)
                    {   
                        if (!appointmentsStartTime.Contains((TimeSpan)startTime))
                        {
                            var appointment = new Appointment();
                            appointment.Doctor = null;
                            appointment.Insured = null;
                            appointment.AppointmentDate = appointmentDay;
                            appointment.AppointmentStartTime = (TimeSpan)startTime;
                            appointment.AppointmentEndTime = (TimeSpan)startTime + new TimeSpan(0, appDuration, 0);
                            appointmentSlots.Add(appointment);
                        }
                        startTime += new TimeSpan(0, appDuration, 0);
                    }
                }
            }
            return Ok(appointmentSlots);
        }
    }
}
