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
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate == appointmentDay && appointment.AppointmentStatus == AppointmentStatus.Upcoming)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.AppointmentCost")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                    else if (HttpContext.Current.Request.UrlReferrer.ToString().EndsWith("Appointments"))
                    {
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentStatus == AppointmentStatus.Upcoming)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.AppointmentCost")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                    else if (appDay != "0001-01-01")
                    {
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate == appointmentDay)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.AppointmentCost")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                    else
                    {
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId)
                                                                                                                 .Include("Doctor")
                                                                                                                 .Include("Doctor.WorkingHours")
                                                                                                                 .Include("Doctor.AppointmentCost")
                                                                                                                 .Include("Doctor.User")
                                                                                                                 .Include("Doctor.User.Roles")
                                                                                                                 .Include("Insured")
                                                                                                                 .Include("Insured.User")
                                                                                                                 .Include("Insured.User.Roles")
                                                                                                                 .ToList();
                    }
                }

                if (HttpContext.Current.User.IsInRole("Insured"))
                {
                    if (appDay != "0001-01-01" && doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate == appointmentDay && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty)
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.AppointmentCost")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
                                                  .Include("Insured.User")
                                                  .Include("Insured.User.Roles")
                                                  .ToList();
                    }
                    else if (appDay != "0001-01-01")
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate == appointmentDay)
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.AppointmentCost")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
                                                  .Include("Insured.User")
                                                  .Include("Insured.User.Roles")
                                                  .ToList();
                    }
                    else if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty)
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.AppointmentCost")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
                                                  .Include("Insured.User")
                                                  .Include("Insured.User.Roles")
                                                  .ToList();
                    }
                    else
                    {
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId))
                                                  .Include("Doctor")
                                                  .Include("Doctor.User")
                                                  .Include("Doctor.User.Roles")
                                                  .Include("Doctor.AppointmentCost")
                                                  .Include("Doctor.WorkingHours")
                                                  .Include("Insured")
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

        //public IEnumerable<Appointment> GetDoctorFilteredAppointments(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty, DateTime appointmentDay, string userId)
        //{
        //    IEnumerable<Appointment> appointments;
        //    IEnumerable<ApplicationUser> users;

        //    using (var db = new ApplicationDbContext())
        //    {
        //        if (!string.IsNullOrWhiteSpace(doctorsFirstName) && !string.IsNullOrWhiteSpace(doctorsLastName))
        //        {
        //            users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName) && user.LastName.Contains(doctorsLastName));
        //        }
        //        else if (!string.IsNullOrWhiteSpace(doctorsFirstName))
        //        {
        //            users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName));
        //        }
        //        else if (!string.IsNullOrWhiteSpace(doctorsLastName))
        //        {
        //            users = db.Users.Where(user => user.LastName.Contains(doctorsLastName));
        //        }
        //        else
        //        {
        //            users = db.Users;
        //        }
        //        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && appointment.InsuredId == userId)
        //                                                                                                     .Include("Doctor")
        //                                                                                                     .Include("Doctor.WorkingHours")
        //                                                                                                     .Include("Doctor.AppointmentCost")
        //                                                                                                     .Include("Doctor.User")
        //                                                                                                     .Include("Insured")
        //                                                                                                     .Include("Insured.User")
        //                                                                                                     .ToList();

        //        if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
        //        {
        //            appointments = appointments.Where(appointment => (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty).ToList();
        //        }

        //        var appDay = appointmentDay.Date.ToString("yyyy-MM-dd");

        //        if (appDay != "0001-01-01")
        //        {
        //            appointments = appointments.Where(appointment => appointment.AppointmentDate.Date.ToString("yyyy-MM-dd") == appDay).ToList();
        //        }
        //    }

        //    return appointments;

        //}

        //public IEnumerable<Appointment> GetInsuredFilteredAppointments(string insuredsFirstName, string insuredsLastName, DateTime appointmentDate, string userId)
        //{
        //    IEnumerable<Appointment> appointments;
        //    IEnumerable<ApplicationUser> users;

        //    using (var db = new ApplicationDbContext())
        //    {
        //        if (!string.IsNullOrEmpty(insuredsFirstName) && !string.IsNullOrEmpty(insuredsLastName))
        //        {
        //            users = db.Users.Where(user => user.FirstName.Contains(insuredsFirstName) && user.LastName.Contains(insuredsLastName));
        //        }
        //        else if (!string.IsNullOrEmpty(insuredsFirstName))
        //        {
        //            users = db.Users.Where(user => user.FirstName.Contains(insuredsFirstName));
        //        }
        //        else if (!string.IsNullOrEmpty(insuredsLastName))
        //        {
        //            users = db.Users.Where(user => user.LastName.Contains(insuredsLastName));
        //        }
        //        else
        //        {
        //            users = db.Users;
        //        }

        //        appointments = db.Appointments.Where(appointment => appointment.DoctorId == userId && users.Any(user => user.Id == appointment.InsuredId))
        //                                      .Include("Doctor")
        //                                      .Include("Doctor.User")
        //                                      .Include("Doctor.AppointmentCost")
        //                                      .Include("Doctor.WorkingHours")
        //                                      .Include("Insured")
        //                                      .Include("Insured.User")
        //                                      .ToList();

        //        var appDay = appointmentDate.Date.ToString("yyyy-MM-dd");

        //        if (appDay != "0001-01-01")
        //        {
        //            appointments = db.Appointments.Where(appointment => appointment.AppointmentDate.Date.ToString("yyyy-MM-dd") == appDay).ToList();
        //        }

        //        return appointments;
        //    }
        //}

        public Appointment GetAppointmentById(int appointmentId)
        {
            Appointment appointment;
            using (var db = new ApplicationDbContext())
            {
                //appointment = db.Appointments.SingleOrDefault(i => i.Id.Equals(appointmentId));
                appointment = db.Appointments.Include("Doctor")
                                             .Include("Doctor.WorkingHours")
                                             .Include("Doctor.AppointmentCost")
                                             .Include("Doctor.User")
                                             .Include("Doctor.User.Roles")
                                             .Include("Insured")
                                             .Include("Insured.User")
                                             .Include("Insured.InsuredPlan")
                                             .Include("Insured.User.Roles")
                                             .SingleOrDefault(d => d.Id.Equals(appointmentId));
                return appointment;
            }
        }

        public IEnumerable<Appointment> GetDoctorAppointmentsOnDate(DateTime date, string doctorId)
        {
            IEnumerable<Appointment> appointments;
            using (var db = new ApplicationDbContext())
            {
                appointments = db.Appointments.Include("Doctor")
                                              .Include("Doctor.WorkingHours")
                                              .Include("Doctor.AppointmentCost")
                                              .Include("Doctor.User")
                                              .Include("Doctor.User.Roles")
                                              .Include("Insured")
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
                db.SaveChanges();
            }
        }
    }
}