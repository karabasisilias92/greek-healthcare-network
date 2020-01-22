using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class AppointmentsRespository
    {
        public IEnumerable<Appointment> GetFilteredAppointments(string FirstName, string LastName, int doctorsSpecialty, DateTime appointmentDay, string userId)
        {
            IEnumerable<Appointment> appointments = new List<Appointment>();
            IEnumerable<ApplicationUser> users;

            using (var db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(FirstName) && user.LastName.Contains(LastName));
                }
                else if (!string.IsNullOrWhiteSpace(FirstName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(FirstName));
                }
                else if (!string.IsNullOrWhiteSpace(LastName))
                {
                    users = db.Users.Where(user => user.LastName.Contains(LastName));
                }
                else
                {
                    users = db.Users;
                }
                var appDay = appointmentDay.Date.ToString("yyyy-MM-dd");
                if (HttpContext.Current.User.IsInRole("Doctor"))
                {
                    if (appDay != "0001-01-01" && HttpContext.Current.Request.UrlReferrer.ToString().EndsWith("Appointments"))
                    {
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate == appointmentDay && appointment.AppointmentChargePaid == true)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.DoctorPlan")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.InsuredPlan")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                    else if (HttpContext.Current.Request.UrlReferrer.ToString().EndsWith("Appointments"))
                    {
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate >= DateTime.Today && appointment.AppointmentChargePaid == true)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.DoctorPlan")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.InsuredPlan")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                    else if (appDay != "0001-01-01")
                    {
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate == appointmentDay && appointment.AppointmentChargePaid == true)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.DoctorPlan")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.InsuredPlan")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                    else
                    {
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentChargePaid == true)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.DoctorPlan")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.InsuredPlan")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                }

                if (HttpContext.Current.User.IsInRole("Insured"))
                {
                    if (appDay != "0001-01-01" && doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate == appointmentDay && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentChargePaid == true)
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.DoctorPlan")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
                                                  .Include("Insured.InsuredPlan")
                                                  .Include("Insured.User")
                                                  .Include("Insured.User.Roles")
                                                  .ToList();
                    }
                    else if (appDay != "0001-01-01")
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate == appointmentDay && appointment.AppointmentChargePaid == true)
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.DoctorPlan")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
                                                  .Include("Insured.InsuredPlan")
                                                  .Include("Insured.User")
                                                  .Include("Insured.User.Roles")
                                                  .ToList();
                    }
                    else if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentChargePaid == true)
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.DoctorPlan")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
                                                  .Include("Insured.InsuredPlan")
                                                  .Include("Insured.User")
                                                  .Include("Insured.User.Roles")
                                                  .ToList();
                    }
                    else
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentChargePaid == true)
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.DoctorPlan")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
                                                  .Include("Insured.InsuredPlan")
                                                  .Include("Insured.User")
                                                  .Include("Insured.User.Roles")
                                                  .ToList();
                    }
                }
            }

            if (HttpContext.Current.Request.UrlReferrer.ToString().EndsWith("Appointments"))
            {
                appointments = appointments.OrderBy(i => i.AppointmentDate).ThenBy(i => i.AppointmentStartTime);
            }

            if (!HttpContext.Current.Request.UrlReferrer.ToString().EndsWith("Appointments"))
            {
                appointments = appointments.OrderByDescending(i => i.AppointmentDate).ThenByDescending(i => i.AppointmentStartTime);
            }

            return appointments;

        }

        public IEnumerable<Appointment> AdminGetFilteredAppointments(string firstName, string lastName, int doctorsSpecialty, int insuredPlanId, DateTime fromDate, DateTime untilDate)
        {
            IEnumerable<Appointment> appointments = new List<Appointment>();
            IEnumerable<ApplicationUser> users;

            using (var db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(firstName) && user.LastName.Contains(lastName));
                }
                else if (!string.IsNullOrWhiteSpace(firstName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(firstName));
                }
                else if (!string.IsNullOrWhiteSpace(lastName))
                {
                    users = db.Users.Where(user => user.LastName.Contains(lastName));
                }
                else
                {
                    users = db.Users;
                }
                var fromDateString = fromDate.Date.ToString("yyyy-MM-dd");
                var untilDateString = untilDate.Date.ToString("yyyy-MM-dd");
                
                if (fromDateString != "0001-01-01" && untilDateString != "0001-01-01")
                {
                    if (doctorsSpecialty != -2 && insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (doctorsSpecialty != -1 && insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (doctorsSpecialty != -2)
                    {
                        if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.AppointmentDate >= fromDate && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                }
                else if (fromDateString != "0001-01-01")
                {
                    if (doctorsSpecialty != -2 && insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (doctorsSpecialty != -1 && insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate >= fromDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentDate >= fromDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate >= fromDate  && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.AppointmentDate >= fromDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (doctorsSpecialty != -2)
                    {
                        if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentDate >= fromDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate >= fromDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate >= fromDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.AppointmentDate >= fromDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                }
                else if (untilDateString != "0001-01-01")
                {
                    if (doctorsSpecialty != -2 && insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (doctorsSpecialty != -1 && insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (doctorsSpecialty != -2)
                    {
                        if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.AppointmentDate <= untilDate && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                }
                else
                {
                    if (doctorsSpecialty != -2 && insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (doctorsSpecialty != -1 && insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId || user.Id == appointment.DoctorId) && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (doctorsSpecialty != -2)
                    {
                        if (doctorsSpecialty != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                    else if (insuredPlanId != -2 && insuredPlanId != -3)
                    {
                        if (insuredPlanId != -1)
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.Insured.InsuredPlanId == insuredPlanId && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                        else
                        {
                            appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.AppointmentChargePaid == true)
                                                                                                                        .Include("Doctor")
                                                                                                                        .Include("Doctor.WorkingHours")
                                                                                                                        .Include("Doctor.DoctorPlan")
                                                                                                                        .Include("Doctor.User")
                                                                                                                        .Include("Doctor.User.Roles")
                                                                                                                        .Include("Insured")
                                                                                                                        .Include("Insured.InsuredPlan")
                                                                                                                        .Include("Insured.User")
                                                                                                                        .Include("Insured.User.Roles")
                                                                                                                        .ToList();
                        }
                    }
                }
            }
            appointments = appointments.OrderByDescending(i => i.AppointmentDate).ThenByDescending(i => i.AppointmentStartTime);

            return appointments;
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            Appointment appointment;
            using (var db = new ApplicationDbContext())
            {
                //appointment = db.Appointments.SingleOrDefault(i => i.Id.Equals(appointmentId));
                appointment = db.Appointments.Include("Doctor")
                                             .Include("Doctor.WorkingHours")
                                             .Include("Doctor.DoctorPlan")
                                             .Include("Doctor.User")
                                             .Include("Doctor.User.Roles")
                                             .Include("Insured")
                                             .Include("Insured.InsuredPlan")
                                             .Include("Insured.User")
                                             .Include("Insured.InsuredPlan")
                                             .Include("Insured.User.Roles")
                                             .SingleOrDefault(d => d.Id.Equals(appointmentId));                
            }
            return appointment;
        }

        public IEnumerable<Appointment> GetDoctorAppointmentsOnDate(DateTime date, string doctorId)
        {
            IEnumerable<Appointment> appointments;
            using (var db = new ApplicationDbContext())
            {
                appointments = db.Appointments.Include("Doctor")
                                              .Include("Doctor.WorkingHours")
                                              .Include("Doctor.DoctorPlan")
                                              .Include("Doctor.User")
                                              .Include("Doctor.User.Roles")
                                              .Include("Insured")
                                              .Include("Insured.InsuredPlan")
                                              .Include("Insured.User")
                                              .Include("Insured.User.Roles")
                                              .Where(app => app.AppointmentDate == date && app.AppointmentStatus == AppointmentStatus.Upcoming && app.DoctorId.Equals(doctorId))
                                              .ToList();
            }
            return appointments;
        }

        public void CancelAppointment(int appointmentId)
        {
            using (var db = new ApplicationDbContext())
            {
                var appointment = db.Appointments.SingleOrDefault(m => m.Id == appointmentId);
                appointment.AppointmentStatus = AppointmentStatus.Canceled;

                var insured = db.Insureds.SingleOrDefault(m => m.User.Id == appointment.InsuredId);
                insured.BookedAppointments--;
                if ((int)(appointment.AppointmentDate - DateTime.Now.Date).Days < 3 && HttpContext.Current.User.IsInRole("Insured"))
                {
                    insured.RefundPending += appointment.InsuredAppointmentCharge * (Convert.ToDecimal(insured.InsuredPlan.CancelationRefundPercentage) / 100m);
                }
                else
                {
                    insured.RefundPending += appointment.InsuredAppointmentCharge;
                }

                db.SaveChanges();
            }
        }

        public int AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException("appointment");
            }
            int id;
            using (var db = new ApplicationDbContext())
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                id = appointment.Id;
            }
            return id;
        }

        public void DeleteAppointment(int appointmentId)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Appointments.Remove(db.Appointments.Find(appointmentId));
                db.SaveChanges();
            }
        }

        public void UpdateAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException("appointment");
            }
            using (var db = new ApplicationDbContext())
            {
                db.Appointments.Attach(appointment);
                db.Entry(appointment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public int AppointmentEntryExists(string insuredId, string doctorId, DateTime appointmentDate, TimeSpan appointmentStartTime)
        {
            int id;
            using (var db = new ApplicationDbContext())
            {
                var appointment = db.Appointments.SingleOrDefault(app => app.InsuredId.Equals(insuredId) && app.DoctorId.Equals(doctorId)
                                                                         && DbFunctions.DiffDays(app.AppointmentDate, appointmentDate) == 0
                                                                         && DbFunctions.DiffMinutes(app.AppointmentStartTime, appointmentStartTime) == 0);
                if (appointment == null)
                {
                    id = 0;
                }
                else
                {
                    id = appointment.Id;
                }
            }
            return id;
        }

        public List<decimal> CalculateAmountOwedToDoctor(IEnumerable<Doctor> doctors)
        {
            List<decimal> doctorsPayments = new List<decimal>();
            using (var db = new ApplicationDbContext())
            {
                foreach (var doctor in doctors)
                {
                    doctorsPayments.Add(doctor.DoctorPlan.AppointmentCost * db.Appointments.Count(app => app.DoctorId.Equals(doctor.UserId) && app.AppointmentStatus == AppointmentStatus.Completed && app.PaidByCompany == false));
                }
            }
            return doctorsPayments;
        }

        public void SetCompletedAppointmentsPaid(string doctorId)
        {
            using (var db = new ApplicationDbContext())
            {
                var appointments = db.Appointments.Where(app => app.DoctorId.Equals(doctorId) && app.AppointmentStatus == AppointmentStatus.Completed && app.PaidByCompany == false);
                foreach (var appointment in appointments)
                {
                    appointment.PaidByCompany = true;
                }
                db.SaveChanges();
            }
        }

        public IEnumerable<Appointment> GetAppointmentsForPeriod(DateTime unavailableFromDate, TimeSpan unavailableFromTime, DateTime unavailableUntilDate, TimeSpan unavailableUntilTime, string doctorId)
        {
            IEnumerable<Appointment> appointments;
            using (var db = new ApplicationDbContext())
            {
                appointments = db.Appointments.Include("Doctor")
                                              .Include("Doctor.WorkingHours")
                                              .Include("Doctor.DoctorPlan")
                                              .Include("Doctor.User")
                                              .Include("Doctor.User.Roles")
                                              .Include("Insured")
                                              .Include("Insured.InsuredPlan")
                                              .Include("Insured.User")
                                              .Include("Insured.User.Roles")
                                              .Where(app => (app.DoctorId.Equals(doctorId) && app.AppointmentStatus == AppointmentStatus.Upcoming && ((DbFunctions.DiffDays(unavailableFromDate, app.AppointmentDate) == 0 && DbFunctions.DiffMinutes(unavailableFromTime, app.AppointmentStartTime) >= 0) || (DbFunctions.DiffDays(unavailableFromDate, app.AppointmentDate) > 0 && DbFunctions.DiffDays(unavailableUntilDate, app.AppointmentDate) < 0) ||  (DbFunctions.DiffDays(unavailableUntilDate, app.AppointmentDate) == 0 && DbFunctions.DiffMinutes(app.AppointmentStartTime, unavailableUntilTime) > 0)))).ToList();
            }
            return appointments;
        }
    }
}