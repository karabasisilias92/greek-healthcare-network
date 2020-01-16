namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDoctorUnavailabilityEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DoctorsUnavailabilities", "UnavailableFromDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.DoctorsUnavailabilities", "UnavailableFromTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.DoctorsUnavailabilities", "UnavailableUntilDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.DoctorsUnavailabilities", "UnavailableUntilTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.DoctorsUnavailabilities", "UnavailableFrom");
            DropColumn("dbo.DoctorsUnavailabilities", "UnavailableUntil");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DoctorsUnavailabilities", "UnavailableUntil", c => c.DateTime(nullable: false));
            AddColumn("dbo.DoctorsUnavailabilities", "UnavailableFrom", c => c.DateTime(nullable: false));
            DropColumn("dbo.DoctorsUnavailabilities", "UnavailableUntilTime");
            DropColumn("dbo.DoctorsUnavailabilities", "UnavailableUntilDate");
            DropColumn("dbo.DoctorsUnavailabilities", "UnavailableFromTime");
            DropColumn("dbo.DoctorsUnavailabilities", "UnavailableFromDate");
        }
    }
}
