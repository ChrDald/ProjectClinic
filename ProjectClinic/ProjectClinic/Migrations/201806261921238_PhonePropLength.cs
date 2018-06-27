namespace ProjectClinic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhonePropLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Doctors", "Phone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Patients", "Phone", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "Phone", c => c.String(maxLength: 10));
            AlterColumn("dbo.Doctors", "Phone", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
