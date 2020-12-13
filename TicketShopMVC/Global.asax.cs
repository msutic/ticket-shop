using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TicketShopMVC.Models;

namespace TicketShopMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            List<User> users = FileOperations.ReadUsers("~/App_Data/users.txt");
            HttpContext.Current.Application["users"] = users;

            List<Manifestation> manifestations = FileOperations.ReadManifestations("~/App_Data/manifestations.txt");
            manifestations.Sort((x, y) => x.DateAndTime.CompareTo(y.DateAndTime));
            HttpContext.Current.Application["manifestations"] = manifestations;
        }
    }
}
