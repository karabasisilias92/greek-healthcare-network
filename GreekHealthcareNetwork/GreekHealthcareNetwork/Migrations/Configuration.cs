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


            if (!userManager.Users.Any(i => i.UserName == "admin"))
            {
                var result = userManager.Create(new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@ghn.gr",
                    FirstName = "John",
                    LastName = "Smith",
                    PhoneNumber = "+30 6924771994",
                    DoB = new DateTime(1992, 1, 24).Date,
                    AMKA = "24019201979"
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
                    MedicalSpecialty = MedicalSpecialty.Cardiologist,
                    Fee = 100
                });
                context.DoctorPlans.Add(new DoctorPlan()
                {
                    MedicalSpecialty = MedicalSpecialty.Pathologist,
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
                    AMKA = "24019201978",
                    PaypalAccount = "456878900785634",
                    ProfilePicture = "doctor1.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor2@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Pathologist,
                        OfficeAddress = "Arx. Makariou 14",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Pathologist).Id
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
                    AMKA = "24019201978",
                    PaypalAccount = "456878900785634",
                    ProfilePicture = "doctor2.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor3@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Cardiologist,
                        OfficeAddress = "Arx. Makariou 14",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Cardiologist).Id
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
                    AMKA = "24019201978",
                    PaypalAccount = "456878900785634",
                    ProfilePicture = "doctor3.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor4@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Allergist,
                        OfficeAddress = "Arx. Makariou 14",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Allergist).Id
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
                    AMKA = "24019201978",
                    PaypalAccount = "456878900785634",
                    ProfilePicture = "doctor4.jpg"
                }, "!Doctor123");

                if (result.Succeeded)
                {
                    var u = userManager.FindByName("doctor5@ghn.gr");
                    userManager.AddToRole(u.Id, "Doctor");
                    context.Doctors.Add(new Doctor()
                    {
                        UserId = u.Id,
                        MedicalSpecialty = MedicalSpecialty.Pathologist,
                        OfficeAddress = "Arx. Makariou 14",
                        DoctorPlanId = context.DoctorPlans.SingleOrDefault(i => i.MedicalSpecialty == MedicalSpecialty.Pathologist).Id
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
                    AMKA = "24019201979"
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

            #region Appointments Seed

            if (context.Appointments.Count() == 0)
            {
                context.Appointments.Add(new Appointment
                {
                    AppointmentDate = new DateTime(2020, 2, 2, 14, 00, 00).Date,
                    AppointmentStartTime = new DateTime(2020, 2, 2, 14, 00, 00).TimeOfDay,
                    AppointmentEndTime = new DateTime(2020, 2, 2, 14, 30, 00).TimeOfDay,
                    DoctorId = "0618d0d9-7fff-49ac-9ac5-6f1c87cb8d53",
                    InsuredId = "e326ebc4-6dd4-4390-9ced-a2676cf399ef",
                    AppointmentStatus = Models.Enums.AppointmentStatus.Upcoming,
                    InsuredComments = "My leg broke"
                });

                context.Appointments.Add(new Appointment
                {
                    AppointmentDate = new DateTime(2019, 2, 2, 16, 30, 00).Date,
                    AppointmentStartTime = new DateTime(2019, 2, 2, 16, 30, 00).TimeOfDay,
                    AppointmentEndTime = new DateTime(2019, 2, 2, 17, 00, 00).TimeOfDay,
                    DoctorId = "3b4d5d50-664f-4979-8353-99147fe645ce",
                    InsuredId = "e326ebc4-6dd4-4390-9ced-a2676cf399ef",
                    AppointmentStatus = Models.Enums.AppointmentStatus.Completed,
                    InsuredComments = "Fever"
                });

                context.Appointments.Add(new Appointment
                {
                    AppointmentDate = new DateTime(2019, 10, 10, 15, 00, 00).Date,
                    AppointmentStartTime = new DateTime(2019, 10, 10, 15, 00, 00).TimeOfDay,
                    AppointmentEndTime = new DateTime(2019, 10, 10, 15, 30, 00).TimeOfDay,
                    DoctorId = "63e2d557-dc7c-467e-9b89-d6794e4a3876",
                    InsuredId = "e326ebc4-6dd4-4390-9ced-a2676cf399ef",
                    AppointmentStatus = Models.Enums.AppointmentStatus.Canceled,
                    InsuredComments = "Heart attack"
                });

                context.Appointments.Add(new Appointment
                {
                    AppointmentDate = new DateTime(2020, 1, 12, 09, 00, 00).Date,
                    AppointmentStartTime = new DateTime(2020, 1, 12, 09, 00, 00).TimeOfDay,
                    AppointmentEndTime = new DateTime(2020, 1, 12, 09, 30, 00).TimeOfDay,
                    DoctorId = "b9211f10-5322-4b83-808c-72a9a67d2937",
                    InsuredId = "e326ebc4-6dd4-4390-9ced-a2676cf399ef",
                    AppointmentStatus = Models.Enums.AppointmentStatus.Upcoming,
                    InsuredComments = "Vomit"
                });

                context.Appointments.Add(new Appointment
                {
                    AppointmentDate = new DateTime(2019, 08, 22, 12, 00, 00).Date,
                    AppointmentStartTime = new DateTime(2019, 08, 22, 12, 00, 00).TimeOfDay,
                    AppointmentEndTime = new DateTime(2019, 08, 22, 12, 30, 00).TimeOfDay,
                    DoctorId = "8cb05f95-5cd1-48cf-a234-b9636aefd2d9",
                    InsuredId = "e326ebc4-6dd4-4390-9ced-a2676cf399ef",
                    AppointmentStatus = Models.Enums.AppointmentStatus.Completed,
                    InsuredComments = "Nothing"
                });

            }

            #endregion
        }
    }
}


