using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GreekHealthcareNetwork.Repositories
{
    public class DoctorsRepository
    {
        public IEnumerable<Doctor> GetFilteredDoctors(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty)
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
                doctors = db.Doctors.Where(doctor => users.Any(user => user.IsActive == true && user.Id == doctor.UserId)).Include("User")
                                                                                                 // Needs to be lazy loaded even though we do not need it here. We may implement searching for messages, 
                                                                                                 // so we cannot json ignore it in general
                                                                                                 .Include("User.Messages") 
                                                                                                 .Include("WorkingHours")
                                                                                                 .Include("AppointmentCost")
                                                                                                 .OrderBy(i => i.MedicalSpecialty)
                                                                                                 .ThenBy(i => i.User.LastName)
                                                                                                 .ThenBy(i => i.User.FirstName)
                                                                                                 .ToList();

                if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                {
                    doctors = doctors.Where(doctor => (int)doctor.MedicalSpecialty == doctorsSpecialty).ToList();
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
                                   .Include("WorkingHours")
                                   .Include("AppointmentCost")
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
                appointmentCost = db.AppointmentCostPerSpecialty.SingleOrDefault(appCost => appCost.MedicalSpecialty == medicalSpecialty).AppointmentCost;
            }
            return appointmentCost;
        }
    }
}