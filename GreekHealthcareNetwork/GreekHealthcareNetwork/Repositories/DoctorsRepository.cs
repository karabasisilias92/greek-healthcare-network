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
                if (!doctorsFirstName.Equals("") && !doctorsLastName.Equals(""))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName) && user.LastName.Contains(doctorsLastName));
                } 
                else if (!doctorsFirstName.Equals(""))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(doctorsFirstName));
                }
                else if (!doctorsLastName.Equals(""))
                {
                    users = db.Users.Where(user => user.LastName.Contains(doctorsLastName));
                }
                else
                {
                    users = db.Users;
                }
                doctors = db.Doctors.Where(doctor => users.Any(user => user.Id == doctor.UserId)).Include("User").ToList();
                if (doctorsSpecialty != -1)
                {
                    doctors = doctors.Where(doctor => (int)doctor.MedicalSpecialty == doctorsSpecialty);
                }
            }

            return doctors;
        }
    }
}