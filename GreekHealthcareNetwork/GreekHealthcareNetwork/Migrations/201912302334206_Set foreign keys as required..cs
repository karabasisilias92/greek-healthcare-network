namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Setforeignkeysasrequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorsUnavailabilities", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.WorkingHours", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Appointments", "InsuredId", "dbo.Insureds");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "InsuredId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.DoctorsUnavailabilities", new[] { "DoctorId" });
            DropIndex("dbo.WorkingHours", new[] { "DoctorId" });
            DropPrimaryKey("dbo.Messages");
            AlterColumn("dbo.Appointments", "DoctorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Appointments", "InsuredId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.InsuredPlans", "PlanAppoinments", c => c.String(nullable: false));
            AlterColumn("dbo.Messages", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Messages", "ConversationId", c => c.Long(nullable: false));
            AlterColumn("dbo.Messages", "SenderId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Messages", "RecipientId", c => c.String(nullable: false));
            AlterColumn("dbo.DoctorsUnavailabilities", "DoctorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.WorkingHours", "DoctorId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Messages", "Id");
            CreateIndex("dbo.Appointments", "DoctorId");
            CreateIndex("dbo.Appointments", "InsuredId");
            CreateIndex("dbo.Messages", "SenderId");
            CreateIndex("dbo.DoctorsUnavailabilities", "DoctorId");
            CreateIndex("dbo.WorkingHours", "DoctorId");
            AddForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.DoctorsUnavailabilities", "DoctorId", "dbo.Doctors", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.WorkingHours", "DoctorId", "dbo.Doctors", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Appointments", "InsuredId", "dbo.Insureds", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "InsuredId", "dbo.Insureds");
            DropForeignKey("dbo.WorkingHours", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorsUnavailabilities", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.WorkingHours", new[] { "DoctorId" });
            DropIndex("dbo.DoctorsUnavailabilities", new[] { "DoctorId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Appointments", new[] { "InsuredId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropPrimaryKey("dbo.Messages");
            AlterColumn("dbo.WorkingHours", "DoctorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.DoctorsUnavailabilities", "DoctorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Messages", "RecipientId", c => c.String());
            AlterColumn("dbo.Messages", "SenderId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Messages", "ConversationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Messages", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.InsuredPlans", "PlanAppoinments", c => c.String());
            AlterColumn("dbo.Appointments", "InsuredId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Appointments", "DoctorId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Messages", "Id");
            CreateIndex("dbo.WorkingHours", "DoctorId");
            CreateIndex("dbo.DoctorsUnavailabilities", "DoctorId");
            CreateIndex("dbo.Messages", "SenderId");
            CreateIndex("dbo.Appointments", "InsuredId");
            CreateIndex("dbo.Appointments", "DoctorId");
            AddForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Appointments", "InsuredId", "dbo.Insureds", "UserId");
            AddForeignKey("dbo.WorkingHours", "DoctorId", "dbo.Doctors", "UserId");
            AddForeignKey("dbo.DoctorsUnavailabilities", "DoctorId", "dbo.Doctors", "UserId");
            AddForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors", "UserId");
        }
    }
}
