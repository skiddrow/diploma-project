namespace WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FifthMigr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fuelings", "Mileage", c => c.Single(nullable: false));
            AddColumn("dbo.Fuelings", "TankValue", c => c.Single(nullable: false));
            AddColumn("dbo.Liquids", "Mileage", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Liquids", "Mileage");
            DropColumn("dbo.Fuelings", "TankValue");
            DropColumn("dbo.Fuelings", "Mileage");
        }
    }
}
