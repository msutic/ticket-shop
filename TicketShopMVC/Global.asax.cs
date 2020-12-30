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

            List<Ticket> tickets = FileOperations.ReadTickets("~/App_Data/tickets.txt");
            HttpContext.Current.Application["tickets"] = tickets;

            List<Comment> comments = FileOperations.ReadComments("~/App_Data/comments.txt");
            HttpContext.Current.Application["comments"] = comments;

            foreach(Comment c in comments)
            {
                foreach(Manifestation m in manifestations)
                {
                    if (c.ManifestationName.Equals(m.Name))
                        m.Comments.Add(c);
                }
            }

            foreach (Ticket t in tickets)
            {
                foreach(User u in users)
                {
                    if (t.Username.Equals(u.Username))
                    {
                        t.Customer = u;
                    }
                }
            }

            
            foreach(User u in users)
            {
                if (u.Role.Equals(Role.CUSTOMER))
                {
                    foreach (Ticket t in tickets)
                    {
                        foreach (string ti in u.TicketIds)
                        {
                            if (ti.Equals(t.Id))
                            {
                                u.ResrvedTickets.Add(t);
                            }
                        }
                    }
                } else if (u.Role.Equals(Role.SALESMAN))
                {
                    foreach(Manifestation m in manifestations)
                    {
                        foreach(string s in u.ManifestationNames)
                        {
                            if (s.Equals(m.Name))
                            {
                                u.Manifestations.Add(m);
                            }
                        }
                    }
                }
                
            }

            for(int i = 0; i < manifestations.Count; i++)
            {
                for(int j = 0; j < tickets.Count; j++)
                {
                    if (manifestations[i].Name.Equals(tickets[j].ReservedFor.Name))
                    {
                        manifestations[i].seatsAvailable -= 1;
                    }
                }
            }

        }
    }
}
