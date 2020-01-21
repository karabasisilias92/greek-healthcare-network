using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class InsuredsRepository
    {
        public IEnumerable<Insured> GetFilteredInsureds(string insuredsFirstName, string insuredsLastName)
        {
            IEnumerable<Insured> insureds;
            IEnumerable<ApplicationUser> users;
            using (var db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(insuredsFirstName) && !string.IsNullOrWhiteSpace(insuredsLastName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(insuredsFirstName) && user.LastName.Contains(insuredsLastName));
                }
                else if (!string.IsNullOrWhiteSpace(insuredsFirstName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(insuredsFirstName));
                }
                else if (!string.IsNullOrWhiteSpace(insuredsLastName))
                {
                    users = db.Users.Where(user => user.LastName.Contains(insuredsLastName));
                }
                else
                {
                    users = db.Users;
                }
                insureds = db.Insureds.Where(insured => users.Any(user => user.Id == insured.UserId))
                                .Include("User")
                                .Include("User.Roles")
                                .Include("InsuredPlan")
                                .OrderBy(i => i.User.LastName)
                                .ThenBy(i => i.User.FirstName)
                                .ToList();
            }

            return insureds;
        }

        public IEnumerable<Insured> GetFilteredInsureds(string insuredsFirstName, string insuredsLastName, int insuredPlanId)
        {
            IEnumerable<Insured> insureds;
            IEnumerable<ApplicationUser> users;
            using (var db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(insuredsFirstName) && !string.IsNullOrWhiteSpace(insuredsLastName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(insuredsFirstName) && user.LastName.Contains(insuredsLastName));
                }
                else if (!string.IsNullOrWhiteSpace(insuredsFirstName))
                {
                    users = db.Users.Where(user => user.FirstName.Contains(insuredsFirstName));
                }
                else if (!string.IsNullOrWhiteSpace(insuredsLastName))
                {
                    users = db.Users.Where(user => user.LastName.Contains(insuredsLastName));
                }
                else
                {
                    users = db.Users;
                }
                if (insuredPlanId == -1)
                {
                    insureds = db.Insureds.Where(insured => users.Any(user => user.Id == insured.UserId))
                                .Include("User")
                                .Include("User.Roles")
                                .Include("InsuredPlan")
                                .OrderBy(i => i.User.LastName)
                                .ThenBy(i => i.User.FirstName)
                                .ToList();
                }
                else if (insuredPlanId == -3)
                {
                    insureds = db.Insureds.Where(insured => users.Any(user => user.Id == insured.UserId && insured.InsuredPlanId == null))
                                .Include("User")
                                .Include("User.Roles")
                                .Include("InsuredPlan")
                                .OrderBy(i => i.User.LastName)
                                .ThenBy(i => i.User.FirstName)
                                .ToList();
                }
                else
                {
                    insureds = db.Insureds.Where(insured => users.Any(user => user.Id == insured.UserId) && insured.InsuredPlanId == insuredPlanId)
                                .Include("User")
                                .Include("User.Roles")
                                .Include("InsuredPlan")
                                .OrderBy(i => i.User.LastName)
                                .ThenBy(i => i.User.FirstName)
                                .ToList();
                }
                
            }

            return insureds;
        }

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
                if (user.User.SubscriptionEndDate < DateTime.Now)
                {
                    user.BookedAppointments = 0;
                }
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