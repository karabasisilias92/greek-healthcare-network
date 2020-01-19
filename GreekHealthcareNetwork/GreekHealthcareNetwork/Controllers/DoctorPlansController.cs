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
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    public class DoctorPlansController : ApiController
    {
        private readonly PlansRepository _plans = new PlansRepository();
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public IHttpActionResult CreateDoctorPlan(DoctorPlan model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = _plans.InsertDoctorPlan(model);
                }
                catch(Exception)
                {
                    ModelState.AddModelError("", "A plan for this medical specialty has already been created. Please choose another one.");
                    return BadRequest(ModelState);
                }
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpPut]
        [ValidateAntiForgeryToken]
        public IHttpActionResult EditUnavailability(DoctorPlan model)
        {
            if (ModelState.IsValid)
            {
                _plans.EditDoctorPlan(model);
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpDelete]
        [ValidateAntiForgeryToken]
        public IHttpActionResult DeleteDoctorPlan(int id)
        {
            bool result = _plans.DeleteDoctorPlan(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();

        }
    }
}
