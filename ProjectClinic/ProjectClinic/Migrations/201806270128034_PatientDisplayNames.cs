namespace ProjectClinic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientDisplayNames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "DateOfBirth", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "DateOfBirth", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
