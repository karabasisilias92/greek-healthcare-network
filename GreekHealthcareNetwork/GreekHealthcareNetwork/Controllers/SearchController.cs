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

        [HttpGet]
        [Route("api/Search/SearchDoctorResults")]
        public IHttpActionResult SearchDoctorResults(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty)
        {
            return Ok(_doctors.GetFilteredDoctors(doctorsFirstName, doctorsLastName, doctorsSpecialty));
        }
    }
}
