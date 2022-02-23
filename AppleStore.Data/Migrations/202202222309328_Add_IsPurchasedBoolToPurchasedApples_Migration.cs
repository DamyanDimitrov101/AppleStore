namespace AppleStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsPurchasedBoolToPurchasedApples_Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchasedApples", "IsPurchased", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchasedApples", "IsPurchased");
        }
    }
}
