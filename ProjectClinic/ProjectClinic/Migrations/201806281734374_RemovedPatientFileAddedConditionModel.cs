namespace ProjectClinic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPatientFileAddedConditionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            DropTable("dbo.PatientFiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PatientFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Conditions", "PatientId", "dbo.Patients");
            DropIndex("dbo.Conditions", new[] { "PatientId" });
            DropTable("dbo.Conditions");
        }
    }
}
