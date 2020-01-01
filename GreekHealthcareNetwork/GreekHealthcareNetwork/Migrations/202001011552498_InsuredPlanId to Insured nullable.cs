namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsuredPlanIdtoInsurednullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Insureds", "InsuredPlanId", "dbo.InsuredPlans");
            DropIndex("dbo.Insureds", new[] { "InsuredPlanId" });
            AlterColumn("dbo.Insureds", "InsuredPlanId", c => c.Int());
            CreateIndex("dbo.Insureds", "InsuredPlanId");
            Sql("ALTER TABLE Insureds ADD CONSTRAINT FK_Insureds_InsuredPlans_InsuredPlanId FOREIGN KEY (InsuredPlanId) REFERENCES InsuredPlans(Id) ON DELETE SET NULL");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE Insureds DROP CONSTRAINT FK_Insureds_InsuredPlans_InsuredPlanId");
            DropIndex("dbo.Insureds", new[] { "InsuredPlanId" });
            AlterColumn("dbo.Insureds", "InsuredPlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Insureds", "InsuredPlanId");
            AddForeignKey("dbo.Insureds", "InsuredPlanId", "dbo.InsuredPlans", "Id", cascadeDelete: true);
        }
    }
}
