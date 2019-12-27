using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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
                doctors = db.Doctors.Where(doctor => users.Any(user => user.Id == doctor.UserId)).Include("User")
                                                                                                 // Needs to be lazy loaded even though we do not need it here. We may implement searching for messages, 
                                                                                                 // so we cannot json ignore it in general
                                                                                                 .Include("User.Messages") 
                                                                                                 .Include("WorkingHours")
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
                                   .SingleOrDefault(d => d.UserId.Equals(doctorId));
            }
            return doctor;
        }
    }
}