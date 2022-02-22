namespace AppleStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_UserAppleCart_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apples",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 664),
                        ImageUrl = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchasedApples",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Count = c.Int(nullable: false),
                        AppleId = c.String(nullable: false, maxLength: 128),
                        CartId = c.String(nullable: false, maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apples", t => t.AppleId, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .Index(t => t.AppleId)
                .Index(t => t.CartId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.Discounts", t => t.DiscountId, cascadeDelete: true)
                .Index(t => t.DiscountId)
                .Index(t => t.CartId);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Pairs = c.Int(nullable: false),
                        NewPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppleId = c.String(nullable: false, maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apples", t => t.AppleId, cascadeDelete: false)
                .Index(t => t.AppleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CartId = c.String(maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.CartId)
                .Index(t => t.CartId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CartId", "dbo.Carts");
            DropForeignKey("dbo.PurchasedApples", "CartId", "dbo.Carts");
            DropForeignKey("dbo.DiscountsApplieds", "DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.Discounts", "AppleId", "dbo.Apples");
            DropForeignKey("dbo.DiscountsApplieds", "CartId", "dbo.Carts");
            DropForeignKey("dbo.PurchasedApples", "AppleId", "dbo.Apples");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CartId" });
            DropIndex("dbo.Discounts", new[] { "AppleId" });
            DropIndex("dbo.DiscountsApplieds", new[] { "CartId" });
            DropIndex("dbo.DiscountsApplieds", new[] { "DiscountId" });
            DropIndex("dbo.Carts", new[] { "UserId" });
            DropIndex("dbo.PurchasedApples", new[] { "CartId" });
            DropIndex("dbo.PurchasedApples", new[] { "AppleId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Discounts");
            DropTable("dbo.DiscountsApplieds");
            DropTable("dbo.Carts");
            DropTable("dbo.PurchasedApples");
            DropTable("dbo.Apples");
        }
    }
}
