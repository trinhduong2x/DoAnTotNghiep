using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjectLibrary.Database;
using ProjectLibrary.Security;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class BaseController : Controller
    {
        // GET: Administrator/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentSession.Logined == false)
            {
                using (var db = new MyDbDataContext())
                {
                    string cookieClient = Request.Cookies["name_client"].Value;
                    string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
                    string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
                    string computerName = deCodecookieClient.Substring(deCodecookieClient.IndexOf("||") + 2, deCodecookieClient.Length - userName.Length - 2);

                    var user = db.Users.FirstOrDefault(a => a.UserName == userName);
                    if (user == null)
                    {
                        int cout = 0;
                        HttpCookie nameCookie = Request.Cookies["name_client"];
                        while (nameCookie != null)
                        {
                            nameCookie.Expires = DateTime.Now.AddDays(-30);
                            HttpContext.Response.Cookies.Add(nameCookie);
                            cout++;
                            if (cout == 10)
                                break;
                        }
                        filterContext.Result =
                              new RedirectToRouteResult(
                                  new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Administrator" }));
                    }
                    else
                    {
                        CurrentSession.Logined = true;
                    }
                    base.OnActionExecuting(filterContext);
                }    
            }    
        }
    }
}