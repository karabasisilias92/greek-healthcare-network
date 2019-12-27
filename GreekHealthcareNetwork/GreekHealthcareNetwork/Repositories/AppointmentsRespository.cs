using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class AppointmentsRespository
    {
        public IEnumerable<Appointment> GetDoctorFilteredAppointments(string doctorsFirstName, string doctorsLastName, int doctorsSpecialty, DateTime appointmentDay, string userId)
        {
            IEnumerable<Appointment> appointments;
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
                appointments = db.Appointments.Where(appointment => users.Any(user => user.Id == appointment.DoctorId) && appointment.InsuredId == userId)
                                                                                                             .Include("Doctor")
                                                                                                             .Include("Doctor.WorkingHours")
                                                                                                             .Include("Doctor.User")
                                                                                                             .Include("Doctor.User.Messages")
                                                                                                             .Include("Insured")
                                                                                                             .Include("Insured.User")
                                                                                                             .Include("Insured.User.Messages")
                                                                                                             .ToList();

                if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                {
                    appointments = appointments.Where(appointment => (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty).ToList();
                }

                //sINTHIKI GIA DATE DIAFORO TOY 0001-01-01

            }

            return appointments;

        }
        //public IEnumerable<Appointment> GetFilteredAppointments(string firstName, string lastName, int doctorSpecialty, string date)
        //{
        //    IEnumerable<Appointment> appointments;
        //    IEnumerable<Doctor> doctors,

        //    using (var db = new ApplicationDbContext())
        //    {
        //        appointments_list = db.Appointments.Include("Doctor").Include("Insured").ToList();
        //    }

        //    return appointments_list;

        //}
    }
}