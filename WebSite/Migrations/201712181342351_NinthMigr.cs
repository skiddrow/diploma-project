namespace WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NinthMigr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Errors", "CarId", c => c.Int(nullable: false));
            CreateIndex("dbo.Errors", "CarId");
            AddForeignKey("dbo.Errors", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Errors", "CarId", "dbo.Cars");
            DropIndex("dbo.Errors", new[] { "CarId" });
            DropColumn("dbo.Errors", "CarId");
        }
    }
}
