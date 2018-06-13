namespace PrinterySVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigrationPrin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        CustomerNumber = c.Int(nullable: false),
                        EditionNumber = c.Int(nullable: false),
                        TypographerNumber = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateTypographer = c.DateTime(),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.Customers", t => t.CustomerNumber, cascadeDelete: true)
                .ForeignKey("dbo.Editions", t => t.EditionNumber, cascadeDelete: true)
                .ForeignKey("dbo.Typographers", t => t.TypographerNumber)
                .Index(t => t.CustomerNumber)
                .Index(t => t.EditionNumber)
                .Index(t => t.TypographerNumber);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.Editions",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        EditionName = c.String(nullable: false),
                        Coast = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.EditionMaterials",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        EditionNumber = c.Int(nullable: false),
                        MaterialNumber = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.Editions", t => t.EditionNumber, cascadeDelete: true)
                .ForeignKey("dbo.Materials", t => t.MaterialNumber, cascadeDelete: true)
                .Index(t => t.EditionNumber)
                .Index(t => t.MaterialNumber);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        MaterialName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.RackMaterials",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        RackNumber = c.Int(nullable: false),
                        MaterialNumber = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.Materials", t => t.MaterialNumber, cascadeDelete: true)
                .ForeignKey("dbo.Racks", t => t.RackNumber, cascadeDelete: true)
                .Index(t => t.RackNumber)
                .Index(t => t.MaterialNumber);
            
            CreateTable(
                "dbo.Racks",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        RackName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.Typographers",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        TypographerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Number);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "TypographerNumber", "dbo.Typographers");
            DropForeignKey("dbo.RackMaterials", "RackNumber", "dbo.Racks");
            DropForeignKey("dbo.RackMaterials", "MaterialNumber", "dbo.Materials");
            DropForeignKey("dbo.EditionMaterials", "MaterialNumber", "dbo.Materials");
            DropForeignKey("dbo.EditionMaterials", "EditionNumber", "dbo.Editions");
            DropForeignKey("dbo.Bookings", "EditionNumber", "dbo.Editions");
            DropForeignKey("dbo.Bookings", "CustomerNumber", "dbo.Customers");
            DropIndex("dbo.RackMaterials", new[] { "MaterialNumber" });
            DropIndex("dbo.RackMaterials", new[] { "RackNumber" });
            DropIndex("dbo.EditionMaterials", new[] { "MaterialNumber" });
            DropIndex("dbo.EditionMaterials", new[] { "EditionNumber" });
            DropIndex("dbo.Bookings", new[] { "TypographerNumber" });
            DropIndex("dbo.Bookings", new[] { "EditionNumber" });
            DropIndex("dbo.Bookings", new[] { "CustomerNumber" });
            DropTable("dbo.Typographers");
            DropTable("dbo.Racks");
            DropTable("dbo.RackMaterials");
            DropTable("dbo.Materials");
            DropTable("dbo.EditionMaterials");
            DropTable("dbo.Editions");
            DropTable("dbo.Customers");
            DropTable("dbo.Bookings");
        }
    }
}
