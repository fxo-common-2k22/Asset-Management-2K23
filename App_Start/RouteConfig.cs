using System.Web.Mvc;
using System.Web.Routing;

namespace FAPP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            object modules = "Academics|Fee|Global|HRPayroll|HR|Contacts|Setup|Transport|Hostel|Finance|POS|FrontDesk";
            routes.MapRoute("MessagingTemplate", "{module}/Messaging/Template/{id}",
                new { controller = "Messaging", action = "AddEdit", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    { "module", modules }
                }, new[] { "FAPP.Controllers" });

            routes.MapRoute("AddModuleMessaging", "Messaging/AddEdit/{module}/{type}/{id}",
                new { controller = "Messaging", action = "AddEdit", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    { "module", modules }
                }, new[] { "FAPP.Controllers" });
            routes.MapRoute("ModuleMessaging", "Messaging/Send/{module}/{type}/{id}",
                new { controller = "Messaging", action = "Send", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    { "module", modules }
                }, new[] { "FAPP.Controllers" });
            routes.MapRoute("ModuleReportsSetting", "CustomReporting/Setting/{module}/{type}/{id}",
               new { controller = "CustomReporting", action = "Setting", id = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "module", modules }
               }, new[] { "FAPP.Controllers" });

            routes.MapRoute("AddModuleCertificates", "Certificates/AddEdit/{module}/{type}/{id}",
                new { controller = "Certificates", action = "AddEdit", area = "", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    { "module", modules }
                }, new[] { "FAPP.Controllers" });

            routes.MapRoute(
                name: "CertificatesDashboard",
                url: "Certificates/Index",
                defaults: new { controller = "Certificates", action = "Index", area = "", id = UrlParameter.Optional },
                namespaces: new[] { "FAPP.Controllers" });

            routes.MapRoute("ModuleIssuedCertificates", "Certificates/Issued/{module}/{type}/{id}",
                new { controller = "Certificates", action = "Issued", area = "", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    { "module", modules }
                }, new[] { "FAPP.Controllers" });

            routes.MapRoute("ModuleCertificates", "Certificates/Issue/{module}/{type}/{id}",
                new { controller = "Certificates", action = "Issue", area = "", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    { "module", modules }
                }, new[] { "FAPP.Controllers" });



            routes.MapRoute(
                name: "CustomReportingDashboard",
                url: "CustomReporting/Index",
                defaults: new { controller = "CustomReporting", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FAPP.Controllers" });
            routes.MapRoute(
                name: "ReportBuilderAdd",
                url: "CustomReporting/Add/{id}",
                defaults: new { controller = "CustomReporting", action = "Add", id = UrlParameter.Optional },
                namespaces: new[] { "FAPP.Controllers" });

            routes.MapRoute("ModuleReports", "CustomReporting/AddEdit/{module}/{type}/{id}",
                new { controller = "CustomReporting", action = "Add", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    { "module", modules }
                });
            //routes.MapRoute(
            //        name: "ReportBuilderTemplates",
            //        url: "CustomReporting/Templates/{id}",
            //        defaults: new { controller = "CustomReporting", action = "Templates", id = UrlParameter.Optional },
            //        namespaces: new[] { "FAPP.Controllers" });
            routes.MapRoute("AddModuleReports", "CustomReporting/Builder/{module}/{type}/{id}",
               new { controller = "CustomReporting", action = "ReportBuilder", id = UrlParameter.Optional },
               new RouteValueDictionary
               {
                    { "module",modules }
               });

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces : new[] { "FAPP.Controllers" }
            );
        }
    }
}
//using System.Web.Mvc;

//namespace FAPP
//{
//    public class MarkAreaRegistration : AreaRegistration
//    {
//        public override string AreaName
//        {
//            get
//            {
//                return "Mark";
//            }
//        }

//        public override void RegisterArea(AreaRegistrationContext context)
//        {
//            context.MapRoute(
//                "Mark_default",
//                "Mark/{controller}/{action}/{id}",
//                new { action = "Index", id = UrlParameter.Optional }
//            );
//        }
//    }
//}