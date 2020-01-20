using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                user = db.Users.Include("Roles").SingleOrDefault(i => i.Id.Equals(userId));
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

        public void DeactivateUser(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);
                user.IsActive = false;
                db.SaveChanges();
            }
        }

        public void UpdateSubscriptionEndDate(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);
                if (HttpContext.Current.User.IsInRole("Doctor") && !user.SubscriptionEndDate.Date.ToString("yyyy-MM-dd").Equals("0001-01-01") && user.SubscriptionEndDate >= DateTime.Now)
                {
                    user.SubscriptionEndDate = user.SubscriptionEndDate.AddMonths(12);
                }
                else
                {
                    user.SubscriptionEndDate = DateTime.Now.AddMonths(12);
                }
                db.SaveChanges();
            }
        }

        public void UpdateUser(ProfileDetailsViewModel updatedUser)
        {

            using (var db = new ApplicationDbContext())
            {
                db.Users.Attach(updatedUser.User);
                db.Entry(updatedUser.User).State = System.Data.Entity.EntityState.Modified;
                if (HttpContext.Current.User.IsInRole("Administrator"))
                {
                    var userRole = GetRoleNameById(updatedUser.User.Roles.ElementAt(0).RoleId);
                    if (userRole == "Doctor")
                    {
                        updatedUser.Doctor.User = updatedUser.User;
                        db.Doctors.Attach(updatedUser.Doctor);
                        db.Entry(updatedUser.Doctor).State = System.Data.Entity.EntityState.Modified;
                    }
                    if (userRole == "Insured")
                    {
                        updatedUser.Insured.User = updatedUser.User;
                        db.Insureds.Attach(updatedUser.Insured);
                        db.Entry(updatedUser.Insured).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                if (HttpContext.Current.User.IsInRole("Doctor"))
                {
                    updatedUser.Doctor.User = updatedUser.User;
                    db.Doctors.Attach(updatedUser.Doctor);
                    db.Entry(updatedUser.Doctor).State = System.Data.Entity.EntityState.Modified;
                }
                if (HttpContext.Current.User.IsInRole("Insured"))
                {
                    updatedUser.Insured.User = updatedUser.User;
                    db.Insureds.Attach(updatedUser.Insured);
                    db.Entry(updatedUser.Insured).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }
        }

        public void UpdateUser(ApplicationUser updatedUser)
        {

            using (var db = new ApplicationDbContext())
            {
                db.Users.Attach(updatedUser);
                db.Entry(updatedUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public string GetRoleIdByName(string roleName)
        {
            string roleId;
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }
            using (var db = new ApplicationDbContext())
            {
                roleId = db.Roles.SingleOrDefault(r => r.Name.Equals(roleName)).Id;
            }
            return roleId;
        }

        // Gets the role ID of the user and returns it's role
        public string GetRoleNameById(string roleId)
        {
            string roleName;
            if (roleId == null)
            {
                throw new ArgumentNullException("roleId");
            }
            using (var db = new ApplicationDbContext())
            {
                roleName = db.Roles.SingleOrDefault(r => r.Id.Equals(roleId)).Name;
            }
            return roleName;
        }
    }
}