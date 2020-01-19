using GreekHealthcareNetwork.Models;
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
    [System.Web.Mvc.Authorize(Roles = "Doctor")]
    public class DoctorUnavailabilitiesController : ApiController
    {
        private readonly DoctorsRepository _doctors = new DoctorsRepository();

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetUnavailability(int id)
        {
            DoctorsUnavailability entry = _doctors.GetDoctorsUnavailability(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public IHttpActionResult DeclareUnavailability(DoctorsUnavailability model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _doctors.DeclareUnavailability(model);
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpPut]
        [ValidateAntiForgeryToken]
        public IHttpActionResult EditUnavailability(DoctorsUnavailability model)
        {
            if (ModelState.IsValid)
            {
                _doctors.EditUnavailability(model);
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpDelete]
        [ValidateAntiForgeryToken]
        public IHttpActionResult DeleteUnavailability(int id)
        {                
            bool result = _doctors.DeleteUnavailability(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();

        }
    }
}
