using System.Web;
using System.Web.Optimization;

namespace CS_HOSPITALARIO_Front_end
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/jquery-3.3.1.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/vendor.js",
                      "~/Scripts/analytics.js"));

               bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap400b2.css",
                      "~/Content/style-adminator.css",
                      "~/Content/css-keyframes.css",
                      "~/Content/css-jqs.css",
                      "~/Content/css-loader.css"));
        }
    }
}
