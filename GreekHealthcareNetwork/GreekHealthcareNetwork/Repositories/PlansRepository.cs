using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
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
                    db.DoctorPlans.Remove(entry);
                    db.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
    }
}