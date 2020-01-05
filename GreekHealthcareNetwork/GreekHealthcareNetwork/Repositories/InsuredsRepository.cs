using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class InsuredsRepository
    {
        public Insured GetInsuredById(string insuredId)
        {
            Insured insured;
            using (var db = new ApplicationDbContext())
            {
                insured = db.Insureds.Include("InsuredPlan")
                                     .Include("User")
                                     .Include("User.Roles")
                                     .SingleOrDefault(i => i.UserId == insuredId);
            }

            return insured;
        }

        public void InsertInsured(Insured insured)
        {
            if (insured == null)
            {
                throw new ArgumentNullException("insured");
            }

            using (var db = new ApplicationDbContext())
            {
                db.Insureds.Add(insured);
                db.SaveChanges();
            }
        }

        public IEnumerable<InsuredPlan> GetInsuredPlans()
        {
            IEnumerable<InsuredPlan> insuredPlans;

            using (var db = new ApplicationDbContext())
            {
                insuredPlans = db.InsuredPlans.ToList();
            }
            return insuredPlans;
        }

        public InsuredPlan GetInsuredPlan(int planId)
        {
            InsuredPlan insuredPlan;
            if (planId < 1)
            {
                throw new ArgumentException("planId");
            }

            using (var db = new ApplicationDbContext())
            {
                insuredPlan = db.InsuredPlans.Find(planId);
            }
            return insuredPlan;
        }

        public void UpdateInsuredPlan(string userId, int planId)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Insureds.Find(userId);
                user.InsuredPlanId = planId;
                db.SaveChanges();
            }
        }

        public void UpdateInsured(Insured insured)
        {
            if (insured == null)
            {
                throw new ArgumentNullException("insured");
            }
            using (var db = new ApplicationDbContext())
            {
                db.Insureds.Attach(insured);
                db.Entry(insured).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}