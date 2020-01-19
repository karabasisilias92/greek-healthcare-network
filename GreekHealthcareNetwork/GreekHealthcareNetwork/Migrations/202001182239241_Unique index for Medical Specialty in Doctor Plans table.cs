namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueindexforMedicalSpecialtyinDoctorPlanstable : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DoctorPlans", "MedicalSpecialty", true, "MedicalSpecialtyIndex");
        }

        public override void Down()
        {
            DropIndex("dbo.DoctorPlans", "MedicalSpecialtyIndex");
        }
    }
}
