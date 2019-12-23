namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixtypeannotationsfortimeanddate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "AppointmentDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Appointments", "AppointmentStartTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Appointments", "AppointmentEndTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Messages", "SentDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Messages", "SentTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "SentTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Messages", "SentDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "AppointmentEndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "AppointmentStartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "AppointmentDate", c => c.DateTime(nullable: false));
        }
    }
}
