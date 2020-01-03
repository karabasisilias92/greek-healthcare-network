namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMessageRelationships : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "RecipientId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Messages", "RecipientId");
            AddForeignKey("dbo.Messages", "RecipientId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "RecipientId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "RecipientId" });
            AlterColumn("dbo.Messages", "RecipientId", c => c.String(nullable: false));
        }
    }
}
