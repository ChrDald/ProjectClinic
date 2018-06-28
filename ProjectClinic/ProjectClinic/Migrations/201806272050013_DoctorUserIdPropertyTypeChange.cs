namespace ProjectClinic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorUserIdPropertyTypeChange : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE Doctors DROP CONSTRAINT DF__Doctors__UserId__47DBAE45");
            AlterColumn("dbo.Doctors", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Doctors", "UserId", c => c.Int(nullable: false));
        }
    }
}
