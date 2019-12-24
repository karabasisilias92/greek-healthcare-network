namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInsuredInsuredPlansDoctorPlansDbSets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InsuredPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        PlanAppoinments = c.String(),
                        PlanFee = c.Decimal(nullable: false, storeType: "money"),
                        AppointmentRate = c.Double(nullable: false),
                        ExceededAppointmentRate = c.Double(nullable: false),
                        PlanDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DoctorPlans", "MedicalSpecialty", c => c.Int(nullable: false));
            AddColumn("dbo.DoctorPlans", "Fee", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Insureds", "InsuredPlanId", c => c.Int(nullable: false));
            AddColumn("dbo.Insureds", "HomeAddress", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Insureds", "BookedAppointments", c => c.Int(nullable: false));
            CreateIndex("dbo.Insureds", "InsuredPlanId");
            AddForeignKey("dbo.Insureds", "InsuredPlanId", "dbo.InsuredPlans", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Insureds", "InsuredPlanId", "dbo.InsuredPlans");
            DropIndex("dbo.Insureds", new[] { "InsuredPlanId" });
            DropColumn("dbo.Insureds", "BookedAppointments");
            DropColumn("dbo.Insureds", "HomeAddress");
            DropColumn("dbo.Insureds", "InsuredPlanId");
            DropColumn("dbo.DoctorPlans", "Fee");
            DropColumn("dbo.DoctorPlans", "MedicalSpecialty");
            DropTable("dbo.InsuredPlans");
        }
    }
}
