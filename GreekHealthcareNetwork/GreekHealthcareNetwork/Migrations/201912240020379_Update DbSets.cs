namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDbSets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoctorsUnavailabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnavailableFrom = c.DateTime(nullable: false),
                        UnavailableUntil = c.DateTime(nullable: false),
                        DoctorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.WorkingHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        WorkStartTime = c.Time(nullable: false, precision: 7),
                        WorkEndTime = c.Time(nullable: false, precision: 7),
                        AppointmentDuration = c.Byte(nullable: false),
                        DoctorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            AddColumn("dbo.Doctors", "MedicalSpecialty", c => c.Int(nullable: false));
            AddColumn("dbo.Doctors", "OfficeAddress", c => c.String(nullable: false));
            AddColumn("dbo.Doctors", "PaypalAccount", c => c.String(nullable: false));
            AddColumn("dbo.Doctors", "DoctorPlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Doctors", "DoctorPlanId");
            AddForeignKey("dbo.Doctors", "DoctorPlanId", "dbo.DoctorPlans", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkingHours", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorsUnavailabilities", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "DoctorPlanId", "dbo.DoctorPlans");
            DropIndex("dbo.WorkingHours", new[] { "DoctorId" });
            DropIndex("dbo.DoctorsUnavailabilities", new[] { "DoctorId" });
            DropIndex("dbo.Doctors", new[] { "DoctorPlanId" });
            DropColumn("dbo.Doctors", "DoctorPlanId");
            DropColumn("dbo.Doctors", "PaypalAccount");
            DropColumn("dbo.Doctors", "OfficeAddress");
            DropColumn("dbo.Doctors", "MedicalSpecialty");
            DropTable("dbo.WorkingHours");
            DropTable("dbo.DoctorsUnavailabilities");
            DropTable("dbo.DoctorPlans");
        }
    }
}
