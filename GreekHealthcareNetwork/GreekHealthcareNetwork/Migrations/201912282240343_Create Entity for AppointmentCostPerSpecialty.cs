namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateEntityforAppointmentCostPerSpecialty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentCostPerSpecialty",
                c => new
                    {
                        MedicalSpecialty = c.Int(nullable: false),
                        AppointmentCost = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.MedicalSpecialty);
            
            DropColumn("dbo.Doctors", "AppointmentCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "AppointmentCost", c => c.Decimal(nullable: false, storeType: "money"));
            DropTable("dbo.AppointmentCostPerSpecialty");
        }
    }
}
