namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VisitorMessageMessageStatusReplied : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Subject", c => c.String());
            AddColumn("dbo.Messages", "MessageText", c => c.String());
            AddColumn("dbo.Messages", "Email", c => c.String());
            AddColumn("dbo.Messages", "FirstName", c => c.String());
            AddColumn("dbo.Messages", "LastName", c => c.String());
            AddColumn("dbo.Messages", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Discriminator");
            DropColumn("dbo.Messages", "LastName");
            DropColumn("dbo.Messages", "FirstName");
            DropColumn("dbo.Messages", "Email");
            DropColumn("dbo.Messages", "MessageText");
            DropColumn("dbo.Messages", "Subject");
        }
    }
}
