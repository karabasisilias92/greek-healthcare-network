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
        [Route("api/Search/InsuredAppointmentsSearchResults")]
        public IHttpActionResult SearchResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty, DateTime appointmentDay, string userId)
        {
            var appointments = _appointments.GetDoctorFilteredAppointments(doctorsFirstName, doctorsLastName, doctorsSpecialty, appointmentDay, userId);
            if (appointments == null)
            {
                return NotFound();
            }
            return Ok(appointments);
        }
    }
}
