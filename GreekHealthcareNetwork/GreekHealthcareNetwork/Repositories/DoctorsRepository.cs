using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GreekHealthcareNetwork.Repositories
{   
    public class DoctorsRepository
    {
        public IEnumerable<Doctor> AdminGetFilteredDoctors(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty)
        {
            IEnumerable<Doctor> doctors;
            IEnumerable<ApplicationUser> users;
            using (var db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(doctorsFirstName) && !string.IsNullOrWhiteSpace(doctorsLastName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName) && user.LastName.Contains(doctorsLastName));
                } 
                else if (!string.IsNullOrWhiteSpace(doctorsFirstName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName));
                }
                else if (!string.IsNullOrWhiteSpace(doctorsLastName))
                {
                    users = db.Users.Where(user => user.LastName.Contains(doctorsLastName));
                }
                else
                {
                    users = db.Users;
                }
                
                if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                {
                    doctors = db.Doctors.Where(doctor => users.Any(user => user.Id == doctor.UserId) && (int)doctor.MedicalSpecialty == doctorsSpecialty)
                                                                                                    .Include("User")
                                                                                                    .Include("User.Messages")
                                                                                                    .Include("User.Roles")
                                                                                                    .Include("WorkingHours")
                                                                                                    .Include("DoctorPlan")
                                                                                                    .OrderBy(i => i.MedicalSpecialty)
                                                                                                    .ThenBy(i => i.User.LastName)
                                                                                                    .ThenBy(i => i.User.FirstName)
                                                                                                    .ToList();
                }
                else
                {
                    doctors = db.Doctors.Where(doctor => users.Any(user => user.Id == doctor.UserId))
                                                                                                    .Include("User")
                                                                                                    .Include("User.Messages")
                                                                                                    .Include("User.Roles")
                                                                                                    .Include("WorkingHours")
                                                                                                    .Include("DoctorPlan")
                                                                                                    .OrderBy(i => i.MedicalSpecialty)
                                                                                                    .ThenBy(i => i.User.LastName)
                                                                                                    .ThenBy(i => i.User.FirstName)
                                                                                                    .ToList();
                }
            }

            return doctors;
        }

        public IEnumerable<Doctor> GetFilteredDoctors(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty, DateTime appointmentDate)
        {
            IEnumerable<Doctor> doctors;
            IEnumerable<ApplicationUser> users;
            using (var db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(doctorsFirstName) && !string.IsNullOrWhiteSpace(doctorsLastName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName) && user.LastName.Contains(doctorsLastName));
                } 
                else if (!string.IsNullOrWhiteSpace(doctorsFirstName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName));
                }
                else if (!string.IsNullOrWhiteSpace(doctorsLastName))
                {
                    users = db.Users.Where(user => user.LastName.Contains(doctorsLastName));
                }
                else
                {
                    users = db.Users;
                }
                var appDay = appointmentDate.Date.ToString("yyyy-MM-dd");
                if (appDay != "0001-01-01")
                {
                    if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                    {
                        doctors = db.Doctors.Where(doctor => !doctor.DoctorsUnavailability.Any(u => (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) > 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) < 0) 
                                                                                                    || (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) == 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) == 0 && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek && DbFunctions.DiffMinutes(w.WorkStartTime, u.UnavailableFromTime) <= 0 && DbFunctions.DiffMinutes(w.WorkEndTime, u.UnavailableUntilTime) >= 0)) 
                                                                                                    || (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) == 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) < 0 && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek && DbFunctions.DiffMinutes(w.WorkStartTime, u.UnavailableFromTime) <= 0))
                                                                                                    || (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) > 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) == 0 && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek && DbFunctions.DiffMinutes(w.WorkEndTime, u.UnavailableUntilTime) >= 0)))
                                                                                                     && users.Any(user => DbFunctions.DiffDays(appointmentDate, user.SubscriptionEndDate) >= 0 
                                                                                                     && user.IsActive == true && user.Id == doctor.UserId) 
                                                                                                     && (int)doctor.MedicalSpecialty == doctorsSpecialty
                                                                                                     && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek
                                                                                                     && (DbFunctions.DiffMinutes(w.WorkStartTime, w.WorkEndTime) / w.AppointmentDuration) > db.Appointments.Where(app => app.AppointmentStatus == AppointmentStatus.Upcoming 
                                                                                                     && app.DoctorId.Equals(doctor.UserId)
                                                                                                     && DbFunctions.DiffDays(app.AppointmentDate, appointmentDate) == 0
                                                                                                     && DbFunctions.DiffMinutes(w.WorkStartTime, app.AppointmentStartTime) >= 0
                                                                                                     && DbFunctions.DiffMinutes(app.AppointmentEndTime, w.WorkEndTime) >= 0).Count()))
                                                                                                     .Include("User")
                                                                                                     .Include("User.Messages")
                                                                                                     .Include("User.Roles")
                                                                                                     .Include("WorkingHours")
                                                                                                     .Include("DoctorPlan")
                                                                                                     .OrderBy(i => i.MedicalSpecialty)
                                                                                                     .ThenBy(i => i.User.LastName)
                                                                                                     .ThenBy(i => i.User.FirstName)
                                                                                                     .ToList();
                    }
                    else
                    {
                        doctors = db.Doctors.Where(doctor => !doctor.DoctorsUnavailability.Any(u => (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) > 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) < 0)
                                                                                                    || (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) == 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) == 0 && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek && DbFunctions.DiffMinutes(w.WorkStartTime, u.UnavailableFromTime) <= 0 && DbFunctions.DiffMinutes(w.WorkEndTime, u.UnavailableUntilTime) >= 0))
                                                                                                    || (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) == 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) < 0 && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek && DbFunctions.DiffMinutes(w.WorkStartTime, u.UnavailableFromTime) <= 0))
                                                                                                    || (DbFunctions.DiffDays(u.UnavailableFromDate, appointmentDate) > 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) == 0 && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek && DbFunctions.DiffMinutes(w.WorkEndTime, u.UnavailableUntilTime) >= 0)))
                                                                                                     && users.Any(user => DbFunctions.DiffDays(appointmentDate, user.SubscriptionEndDate) >= 0 
                                                                                                     && user.IsActive == true && user.Id == doctor.UserId)
                                                                                                     && doctor.WorkingHours.Any(w => w.Day == appointmentDate.DayOfWeek
                                                                                                     && (DbFunctions.DiffMinutes(w.WorkStartTime, w.WorkEndTime) / w.AppointmentDuration) > db.Appointments.Where(app => app.AppointmentStatus == AppointmentStatus.Upcoming 
                                                                                                     && app.DoctorId.Equals(doctor.UserId)
                                                                                                     && DbFunctions.DiffDays(app.AppointmentDate, appointmentDate) == 0
                                                                                                     && DbFunctions.DiffMinutes(w.WorkStartTime, app.AppointmentStartTime) >= 0
                                                                                                     && DbFunctions.DiffMinutes(app.AppointmentEndTime, w.WorkEndTime) >= 0).Count()))
                                                                                                     .Include("User")
                                                                                                     .Include("User.Messages")
                                                                                                     .Include("User.Roles")
                                                                                                     .Include("WorkingHours")
                                                                                                     .Include("DoctorPlan")
                                                                                                     .OrderBy(i => i.MedicalSpecialty)
                                                                                                     .ThenBy(i => i.User.LastName)
                                                                                                     .ThenBy(i => i.User.FirstName)
                                                                                                     .ToList();
                    }
                }
                else
                {
                    if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                    {
                        doctors = db.Doctors.Where(doctor => users.Any(user => user.IsActive == true && user.Id == doctor.UserId) && (int)doctor.MedicalSpecialty == doctorsSpecialty)
                                                                                                      .Include("User")
                                                                                                      .Include("User.Messages")
                                                                                                      .Include("User.Roles")
                                                                                                      .Include("WorkingHours")
                                                                                                      .Include("DoctorPlan")
                                                                                                      .OrderBy(i => i.MedicalSpecialty)
                                                                                                      .ThenBy(i => i.User.LastName)
                                                                                                      .ThenBy(i => i.User.FirstName)
                                                                                                      .ToList();
                    }
                    else
                    {
                        doctors = db.Doctors.Where(doctor => users.Any(user => user.IsActive == true && user.Id == doctor.UserId))
                                                                                                     .Include("User")
                                                                                                     .Include("User.Messages")
                                                                                                     .Include("User.Roles")
                                                                                                     .Include("WorkingHours")
                                                                                                     .Include("DoctorPlan")
                                                                                                     .OrderBy(i => i.MedicalSpecialty)
                                                                                                     .ThenBy(i => i.User.LastName)
                                                                                                     .ThenBy(i => i.User.FirstName)
                                                                                                     .ToList();
                    }
                }
            }

            return doctors;
        }

        public Doctor GetDoctorById(string doctorId)
        {
            Doctor doctor;
            using (var db = new ApplicationDbContext())
            {
                doctor = db.Doctors.Include("User")
                                   .Include("User.Messages")
                                   .Include("User.Roles")
                                   .Include("WorkingHours")
                                   .Include("DoctorPlan")
                                   .SingleOrDefault(d => d.UserId.Equals(doctorId));
            }
            return doctor;
        }

        public void InsertDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException("doctor");
            }

            using (var db = new ApplicationDbContext())
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
            }
        }

        public IEnumerable<WorkingHours> GetWorkingHours(string doctorId)
        {
            if (doctorId == null)
            {
                throw new ArgumentNullException("doctorId");
            }

            IEnumerable<WorkingHours> workingHours;

            using (var db = new ApplicationDbContext())
            {
                workingHours = db.WorkingHours.Where(i => i.DoctorId.Equals(doctorId)).OrderBy(i => i.Day).ThenBy(i => i.WorkStartTime).ToList();
            }

            return workingHours;
        }

        public void InsertWorkingHoursEntry(WorkingHours workingHoursEntry)
        {
            if (workingHoursEntry == null)
            {
                throw new ArgumentNullException("workingHoursEntry");
            }

            
            using (var db = new ApplicationDbContext())
            {
                
                    db.WorkingHours.Add(workingHoursEntry);
                    db.SaveChanges();
            }
        }

        public void UpdateWorkingHoursEntry(WorkingHours workingHoursEntry)
        {
            if (workingHoursEntry == null)
            {
                throw new ArgumentNullException("workingHoursEntry");
            }
            using (var db = new ApplicationDbContext())
            {
                db.WorkingHours.Attach(workingHoursEntry);
                db.Entry(workingHoursEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteWorkingHoursEntry(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var entry = db.WorkingHours.Find(id);
                if (entry == null)
                {
                    throw new ArgumentException("id");
                }
                var doctorId = entry.DoctorId;
                db.WorkingHours.Remove(entry);
                db.SaveChanges();
                var count = db.WorkingHours.Count(i => i.DoctorId.Equals(doctorId));
                ApplicationUser user;
                if (count == 0)
                {
                    user = db.Users.Find(doctorId);
                    user.IsActive = false;
                }
                db.SaveChanges();
            }
        }

        public int GetDoctorPlanId(MedicalSpecialty? medicalSpecialty)
        {
            int doctorPlanId;

            if (medicalSpecialty == null)
            {
                throw new ArgumentNullException("medicalSpecialty");
            }

            using (var db = new ApplicationDbContext())
            {
                doctorPlanId = db.DoctorPlans.SingleOrDefault(plan => plan.MedicalSpecialty == medicalSpecialty).Id;
            }
            return doctorPlanId;
        }

        public DoctorPlan GetDoctorPlan(string doctorId)
        {
            DoctorPlan doctorPlan;
            if (doctorId == null)
            {
                throw new ArgumentNullException("doctorId");
            }

            using (var db = new ApplicationDbContext())
            {
                doctorPlan = db.Doctors.Include("DoctorPlan").SingleOrDefault(doctor => doctor.UserId == doctorId).DoctorPlan;
            }
            return doctorPlan;
        }

        // If we just want to get appointment cost without fetching from DB all the doctor properties
        public decimal GetAppointmentCostForSpecialty(MedicalSpecialty? medicalSpecialty)
        {
            decimal appointmentCost;

            if (medicalSpecialty == null)
            {
                throw new ArgumentNullException("medicalSpecialty");
            }

            using (var db = new ApplicationDbContext())
            {
                appointmentCost = db.DoctorPlans.SingleOrDefault(appCost => appCost.MedicalSpecialty == medicalSpecialty).AppointmentCost;
            }
            return appointmentCost;
        }

        public IEnumerable<DoctorsUnavailability> GetUnavailabilities(string doctorId)
        {
            if (doctorId == null)
            {
                throw new ArgumentNullException("doctorId");
            }
            IEnumerable<DoctorsUnavailability> doctorsUnavailabilities;
            using ( ApplicationDbContext db = new ApplicationDbContext())
            {
                doctorsUnavailabilities = db.DoctorsUnavailabilities.Where(u => u.DoctorId.Equals(doctorId)).OrderByDescending(i => i.UnavailableFromDate).ThenByDescending(i => i.UnavailableFromTime).ToList();
            }
            return doctorsUnavailabilities;
        }

        public IEnumerable<DoctorsUnavailability> GetUnavailabilitiesForDate(DateTime appointmentDate, string doctorId)
        {
            if (appointmentDate == null)
            {
                throw new ArgumentNullException("appointmentDate");
            }

            if (doctorId == null)
            {
                throw new ArgumentNullException("doctorId");
            }

            IEnumerable<DoctorsUnavailability> doctorsUnavailabilities;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                doctorsUnavailabilities = db.DoctorsUnavailabilities.Where(u => u.DoctorId.Equals(doctorId) && DbFunctions.DiffDays(u.UnavailableFromDate,appointmentDate) >= 0 && DbFunctions.DiffDays(u.UnavailableUntilDate, appointmentDate) <= 0).OrderByDescending(i => i.UnavailableFromDate).ThenByDescending(i => i.UnavailableFromTime).ToList();
            }
            return doctorsUnavailabilities;
        }

        public DoctorsUnavailability GetDoctorsUnavailability(int id)
        {
            DoctorsUnavailability unavailability;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                unavailability = db.DoctorsUnavailabilities.Find(id);
            }
            return unavailability;
        }

        public int DeclareUnavailability(DoctorsUnavailability unavailability)
        {
            if (unavailability == null)
            {
                throw new ArgumentNullException("unavailability");
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.DoctorsUnavailabilities.Add(unavailability);                
                db.SaveChanges();
            }
            return unavailability.Id;
        }

        public void EditUnavailability(DoctorsUnavailability unavailability)
        {
            if (unavailability == null)
            {
                throw new ArgumentNullException("unavailability");
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.DoctorsUnavailabilities.Attach(unavailability);
                db.Entry(unavailability).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool DeleteUnavailability(int id)
        {
            bool result;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var entry = db.DoctorsUnavailabilities.Find(id);
                if (entry == null)
                {
                    result = false;
                }
                else
                {
                    db.DoctorsUnavailabilities.Remove(entry);
                    db.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
    }
}