namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMessageandVisitorMessageEntities : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Messages", "MessageText", c => c.String(nullable: false));
            AlterColumn("dbo.Messages", "FirstName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Messages", "LastName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "LastName", c => c.String());
            AlterColumn("dbo.Messages", "FirstName", c => c.String());
            AlterColumn("dbo.Messages", "MessageText", c => c.String());
            AlterColumn("dbo.Messages", "Subject", c => c.String());
        }
    }
}
