using System.Web.Optimization;

namespace AppleStore.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/css/styles.css",
                "~/Content/site.css"));

            bundles.Add(new Bundle("~/bundles/theme").Include(
                "~/Content/js/scripts.js"));


            bundles.Add(new Bundle("~/bundles/custom").Include(
                "~/Scripts/cardLink.js",
                "~/Scripts/postToAddInCart.js",
                "~/Scripts/cartListDeleteBtn.js",
                "~/Scripts/discountLink.js",
                "~/Scripts/setActiveNavIcon.js",
                "~/Scripts/getNavIconsRoute.js",
                "~/Scripts/productCountSetter.js"));
        }
    }
}
