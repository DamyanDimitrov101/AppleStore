namespace AppleStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDiscountsAppliedTable_Migration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiscountsApplieds", "CartId", "dbo.Carts");
            DropForeignKey("dbo.DiscountsApplieds", "DiscountId", "dbo.Discounts");
            DropIndex("dbo.DiscountsApplieds", new[] { "DiscountId" });
            DropIndex("dbo.DiscountsApplieds", new[] { "CartId" });
            DropTable("dbo.DiscountsApplieds");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DiscountsApplieds",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Count = c.String(),
                        DiscountId = c.String(nullable: false, maxLength: 128),
                        CartId = c.String(nullable: false, maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.DiscountsApplieds", "CartId");
            CreateIndex("dbo.DiscountsApplieds", "DiscountId");
            AddForeignKey("dbo.DiscountsApplieds", "DiscountId", "dbo.Discounts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DiscountsApplieds", "CartId", "dbo.Carts", "Id", cascadeDelete: true);
        }
    }
}
