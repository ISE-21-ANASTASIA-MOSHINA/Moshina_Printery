namespace PrinterySVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        CostEdition = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.EditionMaterials",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        EditionNamber = c.Int(nullable: false),
                        MaterialNamber = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.Editions", t => t.EditionNamber, cascadeDelete: true)
                .ForeignKey("dbo.Materials", t => t.MaterialNamber, cascadeDelete: true)
                .Index(t => t.EditionNamber)
                .Index(t => t.MaterialNamber);
            
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
                        Namber = c.Int(nullable: false, identity: true),
                        RackNamber = c.Int(nullable: false),
                        MaterialNamber = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Namber)
                .ForeignKey("dbo.Materials", t => t.MaterialNamber, cascadeDelete: true)
                .ForeignKey("dbo.Racks", t => t.RackNamber, cascadeDelete: true)
                .Index(t => t.RackNamber)
                .Index(t => t.MaterialNamber);
            
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
            DropForeignKey("dbo.RackMaterials", "RackNamber", "dbo.Racks");
            DropForeignKey("dbo.RackMaterials", "MaterialNamber", "dbo.Materials");
            DropForeignKey("dbo.EditionMaterials", "MaterialNamber", "dbo.Materials");
            DropForeignKey("dbo.EditionMaterials", "EditionNamber", "dbo.Editions");
            DropForeignKey("dbo.Bookings", "EditionNumber", "dbo.Editions");
            DropForeignKey("dbo.Bookings", "CustomerNumber", "dbo.Customers");
            DropIndex("dbo.RackMaterials", new[] { "MaterialNamber" });
            DropIndex("dbo.RackMaterials", new[] { "RackNamber" });
            DropIndex("dbo.EditionMaterials", new[] { "MaterialNamber" });
            DropIndex("dbo.EditionMaterials", new[] { "EditionNamber" });
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
