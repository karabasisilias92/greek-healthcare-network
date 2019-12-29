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
    }
}