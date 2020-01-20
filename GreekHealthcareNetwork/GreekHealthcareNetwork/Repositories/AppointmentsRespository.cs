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
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate == appointmentDay)
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
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate >= DateTime.Today)
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
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId && appointment.AppointmentDate == appointmentDay)
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
                        appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.InsuredId) && appointment.DoctorId == userId)
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
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate == appointmentDay && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty)
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
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && appointment.AppointmentDate == appointmentDay)
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
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId) && (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty)
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
                        appointments = db.Appointments.Where(appointment => appointment.InsuredId == userId && users.Any(user => user.Id == appointment.DoctorId))
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
                throw new ArgumentNullException("insured");
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
    }
}