namespace PrinterySVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableMessageInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.String(),
                        FromMailAddress = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        DateDelivery = c.DateTime(nullable: false),
                        CustomerId = c.Int(),
                        Customer_Number = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Number)
                .Index(t => t.Customer_Number);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageInfoes", "Customer_Number", "dbo.Customers");
            DropIndex("dbo.MessageInfoes", new[] { "Customer_Number" });
            DropTable("dbo.MessageInfoes");
        }
    }
}
