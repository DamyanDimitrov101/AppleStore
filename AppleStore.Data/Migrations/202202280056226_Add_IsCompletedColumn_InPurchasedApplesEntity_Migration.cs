namespace AppleStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsCompletedColumn_InPurchasedApplesEntity_Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchasedApples", "IsCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchasedApples", "IsCompleted");
        }
    }
}
