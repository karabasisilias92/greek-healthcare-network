namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Uniqueindexfornameininsuredplans : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.InsuredPlans", "Name", true, "NameIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.InsuredPlans", "NameIndex");
        }
    }
}
