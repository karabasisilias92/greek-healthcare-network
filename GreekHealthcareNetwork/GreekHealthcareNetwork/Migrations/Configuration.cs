namespace GreekHealthcareNetwork.Migrations
{
    using GreekHealthcareNetwork.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GreekHealthcareNetwork.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GreekHealthcareNetwork.Models.ApplicationDbContext";
        }

        protected override void Seed(GreekHealthcareNetwork.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var store = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(store);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            if (!roleManager.RoleExists("Administrator"))
            {
                roleManager.Create(new IdentityRole { Name = "Administrator" });
            }

            if (!roleManager.RoleExists("Insured"))
            {
                roleManager.Create(new IdentityRole { Name = "Insured" });
            }

            if (!roleManager.RoleExists("Doctor"))
            {
                roleManager.Create(new IdentityRole { Name = "Doctor" });
            }


            if (!userManager.Users.Any(i => i.UserName == "admin@ghn.gr"))
            {
                var result = userManager.Create(new ApplicationUser
                {
                    UserName = "admin@ghn.gr",
                    Email = "admin@ghn.gr",
                    FirstName = "John",
                    LastName = "Smith",
                    PhoneNumber = "+30 6924771994"
                }, "!Admin123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("admin@ghn.gr");
                    userManager.AddToRole(u.Id, "Administrator");
                }

            }


        }
    }
}
