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
                    PhoneNumber = "+30 6924771994",
                    DoB = new DateTime(1992, 1, 24).Date,
                    AMKA = 24019201979
                }, "!Admin123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("admin@ghn.gr");
                    userManager.AddToRole(u.Id, "Administrator");
                }

            }
            if (context.Doctors.Count() == 1)
            {
                context.DoctorPlans.Add(new DoctorPlan()
                {
                    MedicalSpecialty = MedicalSpecialty.Cardiologists,
                    Fee = 100
                });
                context.DoctorPlans.Add(new DoctorPlan()
                {
                    MedicalSpecialty = MedicalSpecialty.Pathologists,
                    Fee = 60
                });
                var result = userManager.Create(new ApplicationUser
                {
                    UserName = "doctor2@ghn.gr",
                    Email = "doctor2@ghn.gr",
                    FirstName = "Jessica",
                    LastName = "Wally",
                    PhoneNumber = "+30 6946540329",
                    DoB = new DateTime(1992, 1, 20).Date,
                    AMKA = 24019201978,
                    ProfilePicture = "doctor1.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor2@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Pathologists,
                        OfficeAddress = "Arx. Makariou 14",
                        PaypalAccount = "456878900785634",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Pathologists).Id
                    });
                }

                result = userManager.Create(new ApplicationUser
                {
                    UserName = "doctor3@ghn.gr",
                    Email = "doctor3@ghn.gr",
                    FirstName = "Iai",
                    LastName = "Donas",
                    PhoneNumber = "+30 6946540329",
                    DoB = new DateTime(1992, 1, 20).Date,
                    AMKA = 24019201978,
                    ProfilePicture = "doctor2.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor3@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Cardiologists,
                        OfficeAddress = "Arx. Makariou 14",
                        PaypalAccount = "456878900785634",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Cardiologists).Id
                    });
                }

                result = userManager.Create(new ApplicationUser
                {
                    UserName = "doctor4@ghn.gr",
                    Email = "doctor4@ghn.gr",
                    FirstName = "Amanda",
                    LastName = "Denyl",
                    PhoneNumber = "+30 6946540329",
                    DoB = new DateTime(1992, 1, 20).Date,
                    AMKA = 24019201978,
                    ProfilePicture = "doctor3.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor4@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Allergists,
                        OfficeAddress = "Arx. Makariou 14",
                        PaypalAccount = "456878900785634",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Allergists).Id
                    });
                }

                result = userManager.Create(new ApplicationUser
                {
                    UserName = "doctor5@ghn.gr",
                    Email = "doctor5@ghn.gr",
                    FirstName = "Jason",
                    LastName = "Davis",
                    PhoneNumber = "+30 6946540329",
                    DoB = new DateTime(1992, 1, 20).Date,
                    AMKA = 24019201978,
                    ProfilePicture = "doctor4.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor5@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Pathologists,
                        OfficeAddress = "Arx. Makariou 14",
                        PaypalAccount = "456878900785634",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Pathologists).Id
                    });
                }
            }
            //
            if (context.Insureds.Count() == 0)
            {
                context.InsuredPlans.Add(new InsuredPlan()
                {
                    Name = "Bronze",
                    PlanFee = 250,
                    AppointmentRate = 100,
                    ExceededAppointmentRate = 30,
                    PlanDuration = 3
                });
                var result = userManager.Create(new ApplicationUser
                {
                    UserName = "insured1@ghn.gr",
                    Email = "insured1@ghn.gr",
                    FirstName = "Hlias",
                    LastName = "Karabasis",
                    PhoneNumber = "+30 6924771234",
                    DoB = new DateTime(1992, 1, 24).Date,
                    AMKA = 24019201979
                }, "!Insured123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("insured1@ghn.gr");
                    userManager.AddToRole(u.Id, "Insured");
                    context.Insureds.Add(new Insured()
                    {
                        UserId = u.Id,
                        HomeAddress = "Arx. Makariou 14"
                    });
                }


            }
        }
    }
}


