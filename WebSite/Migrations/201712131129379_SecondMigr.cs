namespace WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "UserId");
        }
    }
}
