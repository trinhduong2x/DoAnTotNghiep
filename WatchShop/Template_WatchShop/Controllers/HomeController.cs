using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using Template_WatchShop.Models;
namespace Template_WatchShop.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(object aliasMenuSub, object idSub, object aliasSub, int? size, int? page)
        {
            var db = new MyDbDataContext();
            Watchshop watchshop = CommentController.DetailShop();
            ViewBag.MetaTitle = watchshop.MetaTitle;
            ViewBag.MetaDesctiption = watchshop.MetaDescription;
            //list show page
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "9", Value = "9" });
            items.Add(new SelectListItem { Text = "15", Value = "15" });
            items.Add(new SelectListItem { Text = "30", Value = "20" });

            // 1.1. Giữ trạng thái kích thước trang được chọn trên DropDownList
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            // 1.2. Tạo các biến ViewBag
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại

            // 2. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;
            //3. lấy danh sách sp mens

            // 4. Tạo kích thước trang (pageSize), mặc định là 6.
            int pageSize = (size ?? 6);

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            if (aliasMenuSub.ToString() == "System.Object")
            {
                return View("Index");
            }

            // xác định menu => tìm ra Kiểu hiển thị của menu
            Menu menu = db.Menus.FirstOrDefault(a => a.Alias == aliasMenuSub.ToString());
            // xác định categary => tìm ra Kiểu hiển thị của categary
            Categary categary = db.Categaries.FirstOrDefault(a => a.Alias == aliasMenuSub.ToString());
            if (menu == null && categary == null)
            {
                return View("404");
            }            
            
            else  
            {
                if (menu != null)
                {
                    //Seo
                    ViewBag.MetaTitle = menu.MetaTitle;
                    ViewBag.MetaDesctiption = menu.MetaDescription;
                    ViewBag.Menu = menu;
                    switch (menu.Type)
                    {
                        case SystemMenuType.AboutUs:
                            return View("About");
                        case SystemMenuType.Watch:
                            goto TrangWatch;
                        case SystemMenuType.Contact:
                            return View("Contact");
                        case SystemMenuType.Sale:
                            goto TrangSale;
                        case SystemMenuType.News:
                           goto TrangTinTuc;
                        case SystemMenuType.Shipping:
                            return View("Shipping");
                        default:
                            return View("Index");
                    }
                }
                else
                {
                    //Seo
                    ViewBag.MetaTitle = categary.Titel;
                    ViewBag.MetaDesctiption = categary.Titel;
                    ViewBag.Menu = categary;
                    if (idSub.ToString() != "System.Object")
                    {
                        int idWatch;
                        int.TryParse(idSub.ToString(), out idWatch);
                        DetailWatch detailWatch = CommentController.detailWatch(idWatch);
                        ViewBag.MetaTitle = detailWatch.watch.MetaTitle;
                        ViewBag.MetaDesctiption = detailWatch.watch.MetaDescription;
                        return View("Watch/DetailWatch", detailWatch);
                    }
                    //Danh sách đồng hồ
                    List<ShowObject> watchCategary = CommentController.WatchCategary(categary.ID);
                    if(watchCategary ==null)
                    {
                        return View("Watch/Message");

                    }
                    if (watchCategary.Count == 1)
                    {
                        DetailWatch detailWatch = CommentController.detailWatch(watchCategary[0].ID);
                        ViewBag.MetaTitle = detailWatch.watch.MetaTitle;
                        ViewBag.MetaDesctiption = detailWatch.watch.MetaDescription;
                        return View("Watch/DetailWatch", detailWatch);
                    }
                    return View("Watch/ListWatch", watchCategary.ToPagedList(pageNumber, pageSize));

                }

            }




        #region "Trang Tin tức"
        TrangTinTuc:
            
            if (idSub.ToString() != "System.Object")
            {
                int idArticle;
                int.TryParse(idSub.ToString(), out idArticle);
                DetailArticle detailArticle = CommentController.Detail_Article(idArticle);
                ViewBag.MetaTitle = detailArticle.Article.MetaTitle;
                ViewBag.MetaDesctiption = detailArticle.Article.MetaDescription;
                return View("Article/DetailArticle", detailArticle);
            }
            //Danh sách đồng hồ
            List<Article> articles = db.Articles.Where(a => a.Status).ToList();
            if (articles.Count == 1)
            {
                DetailArticle detailArticle = CommentController.Detail_Article(articles[0].ID);
                ViewBag.MetaTitle = detailArticle.Article.MetaTitle;
                ViewBag.MetaDesctiption = detailArticle.Article.MetaDescription;
                return View("Article/DetailArticle", detailArticle);
            }
            return View("Article/ListArticle", articles);
        #endregion


        #region "Trang đồng hồ"
        TrangWatch:
           
            //Danh sách đồng hồ
            List<ShowObject> watchs = CommentController.GetAllWatches();
            if(watchs == null)
            {
                return View("Watch/Message");

            }
            if (watchs.Count == 1)
            {
                DetailWatch detailWatch = CommentController.detailWatch(watchs[0].ID);
                ViewBag.MetaTitle = detailWatch.watch.MetaTitle;
                ViewBag.MetaDesctiption = detailWatch.watch.MetaDescription;
                return View("Watch/DetailWatch", detailWatch);
            }
            return View("Watch/ListWatch", watchs.ToPagedList(pageNumber, pageSize));
        #endregion

        #region "Trang đồng hồ giảm giá"
        TrangSale:

            //Danh sách đồng hồ
            List<ShowObject> watchsale = CommentController.GetSaleWatches();
            if (watchsale.Count == 1)
            {
                DetailWatch detailWatch = CommentController.detailWatch(watchsale[0].ID);
                ViewBag.MetaTitle = detailWatch.watch.MetaTitle;
                ViewBag.MetaDesctiption = detailWatch.watch.MetaDescription;
                return View("Watch/DetailWatch", detailWatch);
            }
            return View("Watch/ListWatch", watchsale.ToPagedList(pageNumber, pageSize));
            #endregion
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            var db = new MyDbDataContext();
            if (!string.IsNullOrEmpty(Request.Params["search"]))
            {
                string name = Request.Params["search"];
                Categary categaries = db.Categaries.Where(a => a.Titel.ToUpper().Contains(search.ToUpper())).FirstOrDefault();
                Watch watch = db.Watches.Where(a => a.Name.ToUpper().Contains(search.ToUpper())).FirstOrDefault();
                if (categaries == null && watch == null)
                {
                    return View("Search/View");
                }    
                if(categaries != null)
                {
                    List<ShowObject> watchCategary = CommentController.WatchCategary(categaries.ID);
                    if (watchCategary.Count == 1)
                    {
                        DetailWatch detailWatch = CommentController.detailWatch(watchCategary[0].ID);
                        ViewBag.MetaTitle = detailWatch.watch.MetaTitle;
                        ViewBag.MetaDesctiption = detailWatch.watch.MetaDescription;
                        return View("Search/Detail", detailWatch);
                    }
                    return View("Search/List", watchCategary);
                } 
                if(watch != null)
                {
                    List<ShowObject> watchCategary = CommentController.WatchCategary(watch.CatageryID);
                    DetailWatch detailWatch = CommentController.detailWatch(watchCategary[0].ID);
                    ViewBag.MetaTitle = detailWatch.watch.MetaTitle;
                    ViewBag.MetaDesctiption = detailWatch.watch.MetaDescription;
                    return View("Search/Detail", detailWatch);

                }
               
            }
            return View("Index");

        }

        public ActionResult SearchType(object aliasMenuSub, int search, int? size, int? page)
        {
            var db = new MyDbDataContext();
            Watchshop watchshop = CommentController.DetailShop();
            ViewBag.MetaTitle = watchshop.MetaTitle;
            ViewBag.MetaDesctiption = watchshop.MetaDescription;
            //list show page
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "9", Value = "9" });
            items.Add(new SelectListItem { Text = "15", Value = "15" });
            items.Add(new SelectListItem { Text = "30", Value = "20" });

            // 1.1. Giữ trạng thái kích thước trang được chọn trên DropDownList
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            // 1.2. Tạo các biến ViewBag
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại

            // 2. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;
            //3. lấy danh sách sp mens

            // 4. Tạo kích thước trang (pageSize), mặc định là 6.
            int pageSize = (size ?? 6);

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            if (aliasMenuSub.ToString() == "System.Object")
            {
                return View("Index");
            }

            // xác định menu => tìm ra Kiểu hiển thị của menu
            Menu menu = db.Menus.FirstOrDefault(a => a.Alias == aliasMenuSub.ToString());
            // xác định categary => tìm ra Kiểu hiển thị của categary
            Categary categary = db.Categaries.FirstOrDefault(a => a.Alias == aliasMenuSub.ToString());
            if (menu == null && categary == null)
            {
                return View("404");
            }
           
            else
            {
                if (menu != null)
                {
                    //Seo
                    ViewBag.MetaTitle = menu.MetaTitle;
                    ViewBag.MetaDesctiption = menu.MetaDescription;
                    ViewBag.Menu = menu;
                    List<ShowObject> watchs = CommentController.GetAllWatchesType(search);
                    if (watchs == null)
                    {
                        return View("Watch/Message");

                    }
                    else
                    {
                        return View("Watch/ListWatch", watchs.ToPagedList(pageNumber, pageSize));
                    }
                }
                else
                {
                    //Seo
                    ViewBag.MetaTitle = categary.Titel;
                    ViewBag.MetaDesctiption = categary.Titel;
                    ViewBag.Menu = categary;
                   
                    List<ShowObject> watchCategary = CommentController.WatchCategary(categary.ID,search);
                    if (watchCategary == null)
                    {
                        return View("Watch/Message");
                    }
                    else
                    {
                        return View("Watch/ListWatch", watchCategary.ToPagedList(pageNumber, pageSize));
                    }
                }

            }
        }
    }
}