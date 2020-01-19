namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroptableAppointmentCostPerSpecialtyAddappointmentcosttodoctorplan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Doctors", "MedicalSpecialty", "dbo.AppointmentCostPerSpecialty");
            DropIndex("dbo.Doctors", new[] { "MedicalSpecialty" });
            AddColumn("dbo.DoctorPlans", "AppointmentCost", c => c.Decimal(nullable: false, storeType: "money"));
            DropTable("dbo.AppointmentCostPerSpecialty");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AppointmentCostPerSpecialty",
                c => new
                    {
                        MedicalSpecialty = c.Int(nullable: false),
                        AppointmentCost = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.MedicalSpecialty);
            
            DropColumn("dbo.DoctorPlans", "AppointmentCost");
            CreateIndex("dbo.Doctors", "MedicalSpecialty");
            AddForeignKey("dbo.Doctors", "MedicalSpecialty", "dbo.AppointmentCostPerSpecialty", "MedicalSpecialty", cascadeDelete: true);
        }
    }
}
