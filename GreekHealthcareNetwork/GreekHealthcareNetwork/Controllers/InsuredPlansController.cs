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
    public class InsuredPlansController : ApiController
    {
        private readonly PlansRepository _plans = new PlansRepository();
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public IHttpActionResult CreateInsuredPlan(InsuredPlan model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = _plans.InsertInsuredPlan(model);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "A plan with this name has already been created. Please choose another one.");
                    return BadRequest(ModelState);
                }
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpPut]
        [ValidateAntiForgeryToken]
        public IHttpActionResult EditInsuredPlan(InsuredPlan model)
        {
            if (ModelState.IsValid)
            {
                _plans.EditInsuredPlan(model);
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteInsuredPlan(int id)
        {
            bool result = _plans.DeleteInsuredPlan(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();

        }
    }
}
