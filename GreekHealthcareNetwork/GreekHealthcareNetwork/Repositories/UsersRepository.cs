using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}