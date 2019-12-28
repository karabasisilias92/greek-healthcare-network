namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelateDoctorstoAppointmentPerCostSpecialty : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Doctors", "MedicalSpecialty");
            AddForeignKey("dbo.Doctors", "MedicalSpecialty", "dbo.AppointmentCostPerSpecialty", "MedicalSpecialty", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doctors", "MedicalSpecialty", "dbo.AppointmentCostPerSpecialty");
            DropIndex("dbo.Doctors", new[] { "MedicalSpecialty" });
        }
    }
}
