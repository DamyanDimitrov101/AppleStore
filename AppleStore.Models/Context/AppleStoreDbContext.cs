using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using static AppleStore.Common.GlobalConstants.UserRolesConstants;
using static AppleStore.Common.GlobalConstants.UserEmailConstants;
using static AppleStore.Common.GlobalConstants.DataBaseConstants;

namespace AppleStore.Models.Context
{
    public class AppleStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppleStoreDbContext()
            : base(Name, throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Apple> Apples { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<PurchasedApples> PurchasedApples { get; set; }
        public override IDbSet<ApplicationUser> Users { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder
                .Entity<ApplicationUser>()
                .HasOptional(au => au.Cart)
                .WithMany()
                .HasForeignKey(au => au.CartId);

            base.OnModelCreating(builder);
        }

        public static AppleStoreDbContext Create()
        {
            var context = new AppleStoreDbContext();
            context.CreateRolesAndUsers(context);
            return context;
        }

        private void CreateRolesAndUsers(AppleStoreDbContext context)
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            CreateRole(roleManager, AdminRole);

            var user = new ApplicationUser();
            user.UserName = AdminEmail;
            user.Email = AdminEmail;

            string userPWD = "asdasd";
            var chkUser = UserManager.Create(user, userPWD);

            if (chkUser.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, AdminRole);
            }

            CreateRole(roleManager, UserRole);
        }

        private static void CreateRole(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                var role = new ApplicationRole();
                role.Name = roleName;
                roleManager.Create(role);
            }
        }
    }
}
