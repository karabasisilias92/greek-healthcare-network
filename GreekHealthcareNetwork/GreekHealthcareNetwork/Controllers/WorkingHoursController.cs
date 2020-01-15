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
    public class WorkingHoursController : ApiController
    {
        private readonly DoctorsRepository _doctors = new DoctorsRepository();
        private readonly UsersRepository _users = new UsersRepository();

        [System.Web.Http.HttpGet]
        [System.Web.Mvc.Authorize(Roles = "Doctor")]
        public IHttpActionResult GetWorkingHours(string doctorId)
        {
            if (doctorId == null)
            {
                return BadRequest("Wrong doctorId given");
            }

            return Ok(_doctors.GetWorkingHours(doctorId));
        }

        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize(Roles = "Doctor")]
        public IHttpActionResult UpdateWorkingHours(List<WorkingHours> WorkingHours)
        {
            if (ModelState.IsValid)
            {
                if (!CheckIfWorkingHoursEntriesAreValid(WorkingHours))
                {
                    return BadRequest(ModelState);
                }
                if (CheckIfWorkingHourEntriesIntecept(WorkingHours))
                {
                    return BadRequest(ModelState);
                }
                for (int i = 0; i < WorkingHours.Count; i++)
                {
                    var workingHoursEntry = new WorkingHours
                    {
                        Day = WorkingHours[i].Day,
                        WorkStartTime = WorkingHours[i].WorkStartTime,
                        WorkEndTime = WorkingHours[i].WorkEndTime,
                        AppointmentDuration = WorkingHours[i].AppointmentDuration,
                        DoctorId = WorkingHours[i].DoctorId
                    };
                    if (WorkingHours[i].Id == 0)
                    {
                        try
                        {
                            _doctors.InsertWorkingHoursEntry(workingHoursEntry);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", e.Message);
                            return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        workingHoursEntry.Id = WorkingHours[i].Id;
                        try
                        {
                            _doctors.UpdateWorkingHoursEntry(workingHoursEntry);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", e.Message);
                            return BadRequest(ModelState);
                        }
                    }
                    _users.ActivateUser(WorkingHours[0].DoctorId);
                }
                return Ok();
            }
            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Mvc.Authorize(Roles = "Doctor")]
        public IHttpActionResult DeleteWorkingHoursEntry(int id)
        {
            try
            {
                _doctors.DeleteWorkingHoursEntry(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [System.Web.Http.HttpGet] // just to suppress error with not matching correctly to UpdateWorkingHours
        public bool CheckIfWorkingHoursEntriesAreValid(List<WorkingHours> workingHourList)
        {
            bool result = true;
            double minutes;
            for (int i = 0; i < workingHourList.Count; i++)
            {
                minutes = (workingHourList[i].WorkEndTime - workingHourList[i].WorkStartTime).Value.TotalMinutes;
                if (minutes <= 0 || minutes % workingHourList[i].AppointmentDuration != 0)
                {
                    result = false;
                    ModelState.AddModelError("", "Entry " + (i + 1) + ": Work End Time must be greater than Work Start Time and their difference in minutes must be divided exactly by Appointment Duration");
                }
            }
            return result;
        }

        [System.Web.Http.HttpGet] // just to suppress error with not matching correctly to UpdateWorkingHours
        public bool CheckIfWorkingHourEntriesIntecept(List<WorkingHours> workingHoursList)
        {
            bool result = false;
            for (int i = 0; i < workingHoursList.Count; i++)
            {
                for (int j = i + 1; j < workingHoursList.Count; j++)
                {
                    if (workingHoursList[i].Day == workingHoursList[j].Day && ((workingHoursList[j].WorkStartTime <= workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime > workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime <= workingHoursList[i].WorkEndTime) ||
                                                                               (workingHoursList[j].WorkStartTime <= workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime > workingHoursList[i].WorkEndTime) ||
                                                                               (workingHoursList[j].WorkStartTime > workingHoursList[i].WorkStartTime && workingHoursList[j].WorkEndTime <= workingHoursList[i].WorkEndTime) ||
                                                                               (workingHoursList[j].WorkStartTime > workingHoursList[i].WorkStartTime && workingHoursList[j].WorkStartTime < workingHoursList[i].WorkEndTime && workingHoursList[j].WorkEndTime > workingHoursList[i].WorkEndTime)))
                    {
                        result = true;
                        ModelState.AddModelError("", "Entry " + (i + 1) + " is identical or intercepts with entry " + (j + 1) + "! Please check and fix.");
                    }
                }
            }

            return result;
        }
    }
}
