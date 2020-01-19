using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class PlansRepository
    {
        public IEnumerable<DoctorPlan> GetDoctorPlans()
        {
            IEnumerable<DoctorPlan> doctorPlans;
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                doctorPlans = db.DoctorPlans.OrderBy(i => i.MedicalSpecialty).ToList();
            }
            return doctorPlans;
        }

        public int InsertDoctorPlan(DoctorPlan doctorPlan)
        {
            if (doctorPlan == null)
            {
                throw new ArgumentNullException("doctorPlan");
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.DoctorPlans.Add(doctorPlan);
                db.SaveChanges();
            }
            return doctorPlan.Id;
        }

        public void EditDoctorPlan(DoctorPlan doctorPlan) 
        {
            if (doctorPlan == null)
            {
                throw new ArgumentNullException("doctorPlan");
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.DoctorPlans.Attach(doctorPlan);
                db.Entry(doctorPlan).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool DeleteDoctorPlan(int id)
        {
            bool result;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var entry = db.DoctorPlans.Find(id);
                if (entry == null)
                {
                    result = false;
                }
                else
                {
                    var doctorUsers = db.Users.Where(u => db.Doctors.Where(d => d.DoctorPlanId == id).Select(d => d.UserId).Contains(u.Id)).ToList();
                    db.DoctorPlans.Remove(entry);
                    db.SaveChanges();                    
                    foreach(var user in doctorUsers) {
                        var receivedMessages = db.Messages.Where(m => m.RecipientId.Equals(user.Id));
                        foreach(var message in receivedMessages)
                        {
                            db.Messages.Remove(message);
                        }
                        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Content/img/Doctors/" + user.Id));
                        EmptyFolder(directory);
                        Directory.Delete(HttpContext.Current.Server.MapPath("~/Content/img/Doctors/" + user.Id));
                        db.SaveChanges();
                        db.Users.Remove(user);
                        db.SaveChanges();
                    }
                    result = true;
                }
            }
            return result;
        }

        private void EmptyFolder(DirectoryInfo directory)
        {

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                EmptyFolder(subdirectory);
                subdirectory.Delete();
            }

        }
    }
}