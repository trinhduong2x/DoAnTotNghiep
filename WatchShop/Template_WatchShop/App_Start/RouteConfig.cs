using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Template_WatchShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Booking2", "Carts/SendBooking", new
            {
                controller = "Carts",
                action = "SendBooking",
            });
            routes.MapRoute("Booking3", "Carts/Messages", new
            {
                controller = "Carts",
                action = "Messages",
            });
            //Add Cart
            routes.MapRoute("View Cart", "Carts", new
            {
                controller = "Carts",
                action = "Index",
            });
            //
            routes.MapRoute("Add Cart2", "Carts/addItem/{id}", new
            {
                controller = "Carts",
                action = "addItem",
                id = UrlParameter.Optional,
            },
              namespaces: new[] { "Template_WatchShop.Controllers" }
            );
            //
            routes.MapRoute("Delete Cart2", "Carts/Xoa/{id}", new
            {
                controller = "Carts",
                action = "Xoa",
                id = UrlParameter.Optional,
            });
            //
            routes.MapRoute("DeleteAll Cart2", "Carts/XoaTatCa", new
            {
                controller = "Carts",
                action = "XoaTatCa",
            });
            //
            routes.MapRoute("Update Cart2", "Carts/update/{ID}/{Quantity}", new
            {
                controller = "Carts",
                action = "update",
                ID = UrlParameter.Optional,
                Quantity = UrlParameter.Optional,

            });
            //contact
            routes.MapRoute("Contact", "Contact/SubmitContact", new
            {
                controller = "SendContact",
                action = "SubmitContact",
            });
            //contact
            routes.MapRoute("Contact Messages", "Contact/Messages", new
            {
                controller = "SendContact",
                action = "Messages",
            });

            //Search
            routes.MapRoute("Search", "Search/{keyword}", new
            {
                controller = "Home",
                action = "Search",
                keyword = UrlParameter.Optional
            });
            //Search Type
            routes.MapRoute("SearchType", "SearchType/{aliasMenuSub}/{search}", new
            {
                controller = "Home",
                action = "SearchType",
                aliasMenuSub = UrlParameter.Optional,
                search = UrlParameter.Optional,
            });
            
            routes.MapRoute("Cart", "Search/{keyword}", new
            {
                controller = "Home",
                action = "Search",
                keyword = UrlParameter.Optional
            });

            routes.MapRoute("Default", "{aliasMenuSub}/{idSub}/{aliasSub}", new
            {
                controller = "Home",
                action = "Index",
                aliasMenuSub = UrlParameter.Optional,
                idSub = UrlParameter.Optional,
                aliasSub = UrlParameter.Optional
            }
             );
        }
    }
}
