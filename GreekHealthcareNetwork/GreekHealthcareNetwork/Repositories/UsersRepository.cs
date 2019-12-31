using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GreekHealthcareNetwork.Repositories
{
    public class UsersRepository
    {
        public ApplicationUser GetUserById(string userId)
        {
            ApplicationUser user;
            using (var db = new ApplicationDbContext())
            {
                user = db.Users.SingleOrDefault(i => i.Id.Equals(userId));
            }
            return user;
        }

        public void ActivateUser(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);
                user.IsActive = true;
                db.SaveChanges();
            }
        }

        public void UpdateSubscriptionEndDate(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);
                user.SubscriptionEndDate = DateTime.Now.AddMonths(12);
                db.SaveChanges();
            }
        }

        public void UpdateUser(ProfileDetailsViewModel updatedUser)
        {

            using (var db = new ApplicationDbContext())
            {
                db.Users.Attach(updatedUser.User);
                db.Entry(updatedUser.User).State = System.Data.Entity.EntityState.Modified;

                if (HttpContext.Current.User.IsInRole("Doctor"))
                {
                    db.Doctors.Attach(updatedUser.Doctor);
                    db.Entry(updatedUser.Doctor).State = System.Data.Entity.EntityState.Modified;
                }
                if (HttpContext.Current.User.IsInRole("Insured"))
                {
                    db.Insureds.Attach(updatedUser.Insured);
                    db.Entry(updatedUser.Insured).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}