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
        private readonly InsuredsRepository _insureds = new InsuredsRepository();
        private readonly AppointmentsRespository _appointments = new AppointmentsRespository();

        [HttpGet]
        [Route("api/Search/AdminSearchDoctorResults")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult AdminSearchResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty)
        {
            var doctors = _doctors.AdminGetFilteredDoctors(doctorsFirstName, doctorsLastName, doctorsSpecialty);
            if (doctors == null)
            {
                return NotFound();
            }
            var doctorsPayments = new List<decimal>();
            doctorsPayments = _appointments.CalculateAmountOwedToDoctor(doctors);
            return Ok(new { doctors, doctorsPayments });
        }

        [HttpGet]
        [Route("api/Search/SearchDoctorResults")]
        [AllowAnonymous]
        public IHttpActionResult SearchResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty, DateTime appointmentDate)
        {
            if (appointmentDate.Date.ToString("yyyy-MM-dd") != "0001-01-01" && appointmentDate.Date < DateTime.Now.Date)
            {
                return Ok(new List<Doctor>());
            }
            var doctors = _doctors.GetFilteredDoctors(doctorsFirstName, doctorsLastName, doctorsSpecialty, appointmentDate);
            if (doctors == null)
            {
                return NotFound();
            }
            return Ok(doctors);
        }

        [HttpGet]
        [Route("api/Search/SearchInsuredResults")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult SearchResultsInsureds(string insuredsFirstName, string insuredsLastName, int insuredPlanId)
        {
            var insureds = _insureds.GetFilteredInsureds(insuredsFirstName, insuredsLastName, insuredPlanId);
            if (insureds == null)
            {
                return NotFound();
            }
            return Ok(insureds);
        }

        [HttpGet]
        [Route("api/Search/SearchDoctorById/{doctorId}")]
        [AllowAnonymous]
        public IHttpActionResult SearchDoctorById(string doctorId)
        {
            var doctor = _doctors.GetDoctorById(doctorId);
            if (doctor == null)
            {
                return NotFound();
            }
            doctor.WorkingHours = doctor.WorkingHours.OrderBy(i => i.Day).ThenBy(i => i.WorkStartTime).ToList();            
            return Ok(doctor);
        }

        [HttpGet]
        [Route("api/Search/AppointmentsSearchResults")]
        [Authorize]
        public IHttpActionResult SearchResults(string firstName, string lastName, int doctorsSpecialty, DateTime appointmentDay, string userId)
        {
            var appointments = _appointments.GetFilteredAppointments(firstName, lastName, doctorsSpecialty, appointmentDay, userId);
            if (appointments == null)
            {
                return NotFound();
            }
            return Ok(appointments);
        }

        [HttpGet]
        [Route("api/Search/SearchAppointmentById/{appointmentId}")]
        [Authorize]
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
        [Authorize]
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
            var unavailabilityEntries = _doctors.GetUnavailabilitiesForDate(appointmentDay, doctorId).ToList();
            if (appointments == null || appointments.Count() == 0)
            {
                foreach (var item in workingHoursOfDay)
                {
                    var startTime = item.WorkStartTime;
                    var endTime = item.WorkEndTime;
                    var appDuration = item.AppointmentDuration;
                    while (startTime < endTime)
                    {
                        if (!unavailabilityEntries.Any(u => (u.UnavailableFromDate < appointmentDay && appointmentDay < u.UnavailableUntilDate) 
                                                         || (u.UnavailableFromDate == appointmentDay && appointmentDay == u.UnavailableUntilDate && u.UnavailableFromTime <= startTime && startTime < u.UnavailableUntilTime)
                                                         || (u.UnavailableFromDate == appointmentDay && appointmentDay < u.UnavailableUntilDate && u.UnavailableFromTime <= startTime)
                                                         || (u.UnavailableFromDate < appointmentDay && appointmentDay == u.UnavailableUntilDate && startTime < u.UnavailableUntilTime)))
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
            else
            {
                var appointmentsStartTime = appointments.Select(app => app.AppointmentStartTime).ToList();
                foreach (var item in workingHoursOfDay)
                {
                    var startTime = item.WorkStartTime;
                    var endTime = item.WorkEndTime;
                    var appDuration = item.AppointmentDuration;
                    while (startTime < endTime)
                    {   
                        if (!appointmentsStartTime.Contains((TimeSpan)startTime))
                        {
                            if (!unavailabilityEntries.Any(u => (u.UnavailableFromDate < appointmentDay && appointmentDay < u.UnavailableUntilDate)
                                                         || (u.UnavailableFromDate == appointmentDay && appointmentDay == u.UnavailableUntilDate && u.UnavailableFromTime <= startTime && startTime < u.UnavailableUntilTime)
                                                         || (u.UnavailableFromDate == appointmentDay && appointmentDay < u.UnavailableUntilDate && u.UnavailableFromTime <= startTime)
                                                         || (u.UnavailableFromDate < appointmentDay && appointmentDay == u.UnavailableUntilDate && startTime < u.UnavailableUntilTime)))
                            {
                                var appointment = new Appointment();
                                appointment.Doctor = null;
                                appointment.Insured = null;
                                appointment.AppointmentDate = appointmentDay;
                                appointment.AppointmentStartTime = (TimeSpan)startTime;
                                appointment.AppointmentEndTime = (TimeSpan)startTime + new TimeSpan(0, appDuration, 0);
                                appointmentSlots.Add(appointment);
                            }
                        }
                        startTime += new TimeSpan(0, appDuration, 0);
                    }
                }
            }
            return Ok(appointmentSlots);
        }

        [HttpGet]
        [Route("api/Search/GetUsersWithNoOrExpiredSubscription")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult GetUsersWithNoOrExpiredSubscription(string firstName, string lastName, int doctorSpecialty, int insuredPlanId)
        {
            IEnumerable<Doctor> doctors;
            IEnumerable<Insured> insureds;
            if (doctorSpecialty == -2)
            {
                doctors = new List<Doctor>();
            }
            else
            {
                doctors = _doctors.AdminGetFilteredDoctors(firstName, lastName, doctorSpecialty);
            }

            if (insuredPlanId == -2)
            {
                insureds = new List<Insured>();
            }
            else
            {
                insureds = _insureds.GetFilteredInsureds(firstName, lastName, insuredPlanId);
            }

            if (doctors == null && insureds == null)
            {
                return NotFound();
            }
            else if (doctors == null)
            {
                insureds = insureds.Where(i => i.User.SubscriptionEndDate.Date < DateTime.Now.Date);
                return Ok(new { doctors = new List<Doctor>(), insureds });
            }
            else if (insureds == null)
            {
                doctors = doctors.Where(i => i.User.SubscriptionEndDate.Date < DateTime.Now.Date);
                return Ok(new { doctors , insureds = new List<Insured>() });
            }
            else
            {
                doctors = doctors.Where(i => i.User.SubscriptionEndDate.Date < DateTime.Now.Date);
                insureds = insureds.Where(i => i.User.SubscriptionEndDate.Date < DateTime.Now.Date);
                return Ok(new { doctors, insureds});
            }            
        }

        [HttpGet]
        [Route("api/Search/AdminAppointmentsSearchResults")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult SearchResults(string firstName, string lastName, int doctorsSpecialty, int insuredPlanId, DateTime fromDate, DateTime untilDate)
        {
            var appointments = _appointments.AdminGetFilteredAppointments(firstName, lastName, doctorsSpecialty, insuredPlanId, fromDate, untilDate);
            if (appointments == null)
            {
                return NotFound();
            }
            return Ok(appointments);
        }

        [HttpGet]
        [Route("api/Search/GetAppointmentsForPeriod")]
        [Authorize(Roles = "Doctor")]
        public IHttpActionResult GetAppointmentsForPeriod(DateTime unavailableFromDate, TimeSpan unavailableFromTime, DateTime unavailableUntilDate, TimeSpan unavailableUntilTime, string doctorId)
        {
            var appointments = _appointments.GetAppointmentsForPeriod(unavailableFromDate, unavailableFromTime, unavailableUntilDate, unavailableUntilTime, doctorId);
            return Ok(appointments);
        }
    }
}
