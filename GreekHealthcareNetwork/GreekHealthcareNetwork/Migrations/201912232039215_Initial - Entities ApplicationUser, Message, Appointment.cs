namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialEntitiesApplicationUserMessageAppointment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(maxLength: 128),
                        InsuredId = c.String(maxLength: 128),
                        AppointmentDate = c.DateTime(nullable: false),
                        AppointmentStartTime = c.DateTime(nullable: false),
                        AppointmentEndTime = c.DateTime(nullable: false),
                        AppointmentStatus = c.Int(nullable: false),
                        InsuredComments = c.String(),
                        DoctorComments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Insureds", t => t.InsuredId)
                .Index(t => t.DoctorId)
                .Index(t => t.InsuredId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConversationId = c.Int(nullable: false),
                        SenderId = c.String(maxLength: 128),
                        RecipientId = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        SentTime = c.DateTime(nullable: false),
                        MessageStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.Insureds",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "DoB", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "AMKA", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "SubscriptionEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "InsuredId", "dbo.Insureds");
            DropForeignKey("dbo.Insureds", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Insureds", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Doctors", new[] { "UserId" });
            DropIndex("dbo.Appointments", new[] { "InsuredId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropColumn("dbo.AspNetUsers", "SubscriptionEndDate");
            DropColumn("dbo.AspNetUsers", "AMKA");
            DropColumn("dbo.AspNetUsers", "DoB");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.Insureds");
            DropTable("dbo.Messages");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
