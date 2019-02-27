namespace WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdMigr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cars", "Brand", c => c.String(nullable: false));
            AlterColumn("dbo.Cars", "Model", c => c.String(nullable: false));
            AlterColumn("dbo.Cars", "EngineType", c => c.String(nullable: false));
            AlterColumn("dbo.Cars", "GearBoxType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cars", "GearBoxType", c => c.String());
            AlterColumn("dbo.Cars", "EngineType", c => c.String());
            AlterColumn("dbo.Cars", "Model", c => c.String());
            AlterColumn("dbo.Cars", "Brand", c => c.String());
        }
    }
}
