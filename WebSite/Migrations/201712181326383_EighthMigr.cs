namespace WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EighthMigr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Errors", "Mileage", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Errors", "Mileage");
        }
    }
}
