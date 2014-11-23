using System.Web;
using System.Web.Optimization;

namespace Vanderkorn.ER
{
    using System.Collections.Generic;
    using System.IO;

    public class ReplaceContentsBundleBuilder : IBundleBuilder
    {
        private readonly string _find;

        private readonly string _replaceWith;

        private readonly IBundleBuilder _builder;

        public ReplaceContentsBundleBuilder(string find, string replaceWith)
            : this(find, replaceWith, new DefaultBundleBuilder())
        {
        }

        public ReplaceContentsBundleBuilder(string find, string replaceWith, IBundleBuilder builder)
        {
            _find = find;
            _replaceWith = replaceWith;
            _builder = builder;
        }

        public string BuildBundleContent(Bundle bundle, BundleContext context, IEnumerable<BundleFile> files)
        {
            string contents = _builder.BuildBundleContent(bundle, context, files);

            return contents.Replace(_find, _replaceWith);
        }
    }
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                                  "~/Content/site.css",
                                  "~/Content/fontawesome/font-awesome.css",
                                 "~/Content/bootstrap.min.css",
                                  "~/Content/loading-bar.css")
                                //  .Include("~/Content/fontawesome/font-awesome.css", new CssRewriteUrlTransform())
                                  );

         //   bundles.Add(new StyleBundle("~/Content/fontawesome-bundle").Include("~/Content/fontawesome/font-awesome.css"));

          //  bundles.Add(new StyleBundle("~/Content/font-awesome") 
      //  { Builder = new ReplaceContentsBundleBuilder("url('../fonts/", "url('/Content/fonts/") }
            //    .Include("~/Content/font-awesome/font-awesome.min.css"));
            //bundles.Add(new StyleBundle("~/Content/font-awesome") { Builder = new ReplaceContentsBundleBuilder("url('../fonts/", "url('/Content/fonts/") }
            //   .Include("~/Content/font-awesome.css", new CssRewriteUrlTransform()));
            bundles.Add(new ScriptBundle("~/bundles/EReceptionApp")
                                .Include("~/Scripts/jquery-2.1.1.min.js")
                                .Include("~/Scripts/jquery.signalR-2.1.2.min.js")
                                .Include("~/Scripts/angular.min.js")
                               .Include("~/Scripts/angular-route.min.js")
                               .Include("~/Scripts/angular-local-storage.min.js")
                               .Include("~/Scripts/loading-bar.min.js")
                               .Include("~/Scripts/angular-signalr-hub.min.js")
                               .IncludeDirectory("~/Scripts/Controllers", "*.js")
                               .IncludeDirectory("~/Scripts/Factories", "*.js")
                               .IncludeDirectory("~/Scripts/Services", "*.js")
                               .Include("~/Scripts/EReceptionApp.js")
                              );

            BundleTable.EnableOptimizations = false;
        }
    }
}
