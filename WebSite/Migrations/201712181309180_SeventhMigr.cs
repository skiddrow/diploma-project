namespace WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeventhMigr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ErrorTypeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsFixed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ErrorTypes", t => t.ErrorTypeId, cascadeDelete: true)
                .Index(t => t.ErrorTypeId);
            
            CreateTable(
                "dbo.ErrorTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Decsription = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Errors", "ErrorTypeId", "dbo.ErrorTypes");
            DropIndex("dbo.Errors", new[] { "ErrorTypeId" });
            DropTable("dbo.ErrorTypes");
            DropTable("dbo.Errors");
        }
    }
}
