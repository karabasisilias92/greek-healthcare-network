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

        public void UpdateUser(ProfileDetailsViewModel updatedUser)
        {
            ApplicationUser user;
            Doctor doctor;
            Insured insured;

            using (var db = new ApplicationDbContext())
            {
                user = db.Users.Attach(updatedUser.User);
                db.Entry(updatedUser.User).State = System.Data.Entity.EntityState.Modified;

                if (Roles.IsUserInRole("Doctor"))
                {
                    doctor = db.Doctors.Attach(updatedUser.Doctor);
                    db.Entry(updatedUser.Doctor).State = System.Data.Entity.EntityState.Modified;
                }
                if (Roles.IsUserInRole("Insured"))
                {
                    insured = db.Insureds.Attach(updatedUser.Insured);
                    db.Entry(updatedUser.Insured).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}