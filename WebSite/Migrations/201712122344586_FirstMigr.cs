namespace WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Brand = c.String(),
                        Model = c.String(),
                        Year = c.Int(nullable: false),
                        EngineType = c.String(),
                        EngineAmount = c.Single(nullable: false),
                        GearBoxType = c.String(),
                        TankAmount = c.Int(nullable: false),
                        IsCarHasDC = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fuelings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(nullable: false),
                        Value = c.Single(nullable: false),
                        Price = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId);
            
            CreateTable(
                "dbo.Liquids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(nullable: false),
                        LiquidTypeId = c.Int(nullable: false),
                        LiquidAmount = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .ForeignKey("dbo.LiquidTypes", t => t.LiquidTypeId, cascadeDelete: true)
                .Index(t => t.CarId)
                .Index(t => t.LiquidTypeId);
            
            CreateTable(
                "dbo.LiquidTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mileages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(nullable: false),
                        Value = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mileages", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Liquids", "LiquidTypeId", "dbo.LiquidTypes");
            DropForeignKey("dbo.Liquids", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Fuelings", "CarId", "dbo.Cars");
            DropIndex("dbo.Mileages", new[] { "CarId" });
            DropIndex("dbo.Liquids", new[] { "LiquidTypeId" });
            DropIndex("dbo.Liquids", new[] { "CarId" });
            DropIndex("dbo.Fuelings", new[] { "CarId" });
            DropTable("dbo.Mileages");
            DropTable("dbo.LiquidTypes");
            DropTable("dbo.Liquids");
            DropTable("dbo.Fuelings");
            DropTable("dbo.Cars");
        }
    }
}
