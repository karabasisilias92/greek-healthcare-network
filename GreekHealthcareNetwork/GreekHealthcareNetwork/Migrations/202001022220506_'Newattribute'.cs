namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newattribute : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            AddColumn("dbo.Messages", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Messages", "RecipientId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Messages", "RecipientId");
            CreateIndex("dbo.Messages", "ApplicationUser_Id");
            AddForeignKey("dbo.Messages", "RecipientId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "RecipientId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Messages", new[] { "RecipientId" });
            AlterColumn("dbo.Messages", "RecipientId", c => c.String(nullable: false));
            DropColumn("dbo.Messages", "ApplicationUser_Id");
            AddForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
