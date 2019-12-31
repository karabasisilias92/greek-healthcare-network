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
                                                                                                             .Include("Doctor.AppointmentCost")
                                                                                                             .Include("Doctor.User")
                                                                                                             .Include("Insured")
                                                                                                             .Include("Insured.User")
                                                                                                             .ToList();

                if (doctorsSpecialty >= 0 && doctorsSpecialty < Enum.GetNames(typeof(MedicalSpecialty)).Length)
                {
                    appointments = appointments.Where(appointment => (int)appointment.Doctor.MedicalSpecialty == doctorsSpecialty).ToList();
                }

                var appDay = appointmentDay.Date.ToString("yyyy-MM-dd");

                if (appDay != "0001-01-01")
                {
                    appointments = appointments.Where(appointment => appointment.AppointmentDate.Date.ToString("yyyy-MM-dd") == appointmentDay.Date.ToString("yyyy-MM-dd")).ToList();
                }
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
                                             .Include("Insured")
                                             .Include("Insured.User")
                                             .SingleOrDefault(d => d.Id.Equals(appointmentId));
                return appointment;
            }
        }
    }
}