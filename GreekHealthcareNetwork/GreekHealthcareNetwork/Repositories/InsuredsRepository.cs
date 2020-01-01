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
    }
}