using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;

using AppleStore.Services.AutoMapperConfig;

namespace AppleStore.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ContainerConfig.RegisterContainer();
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
