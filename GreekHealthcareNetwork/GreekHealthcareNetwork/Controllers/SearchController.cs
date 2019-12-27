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
        [Route("api/Search/SearchResults")]
        public IHttpActionResult SearchResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty)
        {
            return Ok(_doctors.GetFilteredDoctors(doctorsFirstName, doctorsLastName, doctorsSpecialty));
        }

        [HttpGet]
        [Route("api/Search/InsuredAppointmentsSearchResults")]
        public IHttpActionResult InsuredAppointmentsSearchResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty, DateTime appointmentDay)
        {
            return Ok(_appointments.GetDoctorFilteredAppointments(doctorsFirstName, doctorsLastName, doctorsSpecialty, appointmentDay));
        }
    }
}
