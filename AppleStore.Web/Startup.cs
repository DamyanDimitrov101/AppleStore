using System.Web;
using System.Web.Mvc;

using AppleStore.Contracts.Services;
using AppleStore.Models;
using AppleStore.Models.Context;
using AppleStore.Models.Repositories;
using AppleStore.Services;
using AppleStore.Services.Contracts;

using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppleStore.Web.Startup))]
namespace AppleStore.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AppleStoreDbContext>().AsSelf().InstancePerRequest();
            AppleStoreDbContext.Create();

            builder.RegisterType<EfRepository<Apple>>()
                .As<IRepository<Apple>>().InstancePerRequest();

            builder.RegisterType<EfRepository<Discount>>()
                .As<IRepository<Discount>>().InstancePerRequest();

            builder.RegisterType<EfRepository<Cart>>()
                .As<IRepository<Cart>>().InstancePerRequest();

            builder.RegisterType<EfRepository<PurchasedApples>>()
                   .As<IRepository<PurchasedApples>>().InstancePerRequest();

            builder.RegisterType<EfRepository<ApplicationUser>>()
                .As<IRepository<ApplicationUser>>().InstancePerRequest();

            builder.RegisterType<SqlAppleData>().As<IAppleData>().InstancePerRequest();
            builder.RegisterType<AppleService>().As<IAppleService>().InstancePerRequest();
            builder.RegisterType<CartService>().As<ICartService>().InstancePerRequest();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerRequest();            

            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            
            ConfigureAuth(app);
        }
    }
}
