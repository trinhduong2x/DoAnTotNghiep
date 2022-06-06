using ProjectLibrary.Database;
using ProjectLibrary.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class CommentController : BaseController
    {
        // GET: Administrator/Comment
        //lấy họ tên người đăng nhập
        public static string GetFullName(string cookie)
        {
            using (var db = new MyDbDataContext())
            {
                string cookieClient = cookie;
                string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
                string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
                return db.Users.FirstOrDefault(a => a.UserName == userName).FullName;
            }
        }
        //Show messages
        public static string Messages(object messages)
        {
            if (messages != null)
                return messages.ToString();
            return "";
        }
        public static List<Watch> GetWatches()
        {
            using(var db = new MyDbDataContext())
            {
                List<Watch> watches = db.Watches.Where(a => a.Quality <= 0).ToList();
                return watches;
            }
        }

        public static List<Order> GetOrder()
        {
            using (var db = new MyDbDataContext())
            {
                List<Order> orders = db.Orders.Where(a => a.Completed != 3).ToList();
                return orders;
            }
        }
    }
}