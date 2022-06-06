using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectLibrary.Config;
using Template_WatchShop.Models;

namespace Template_WatchShop.Controllers
{
    public class CommentController : Controller
    {
        //Chi tiết cửa hàng
        public static Watchshop DetailShop()
        {
            using (var db = new MyDbDataContext())
            {
                var watchshop =
                    db.Watchshops.FirstOrDefault() ??
                    new Watchshop();
                return watchshop;
            }
        }

        //Danh sách Main menu
        public static List<Menu> GetMainMenus()
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> menus = db.Menus.Where(a => a.Status && a.Location == SystemMenuLocation.MainMenu).OrderBy(a => a.Index).ToList();
                return menus;
            }
        }
        //Danh sách Second menu
        public static List<Menu> GetSecondMenus()
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> menufooter = db.Menus.Where(a => a.Status && a.Location == SystemMenuLocation.SecondMenu ).OrderBy(a => a.Index).ToList();
                return menufooter;
            }
        }

        //Danh sach menu type
        public static List<Categary> GetCategaries()
        {
            using (var db = new MyDbDataContext())
            {
                List<Categary> categaries = db.Categaries.Where(a => a.Status).OrderBy(a => a.ID).ToList();
                return categaries;
            }
        }

        //Danh sách slide
        public static List<Slide> GetSlides()
        {
            using(var db = new MyDbDataContext())
            {
                List<Slide> slides = db.Slides.Where(a => a.Status).OrderBy(a => a.Index).ToList();
                return slides;
            }
        }

        public static List<TypeWatch> GetTypeWatches()
        {
            using(var db = new MyDbDataContext())
            {
                List<TypeWatch> typeWatches = db.TypeWatches.ToList();
                return typeWatches;
            }
        }

        //Danh sách đồng hồ hot
        public static List<ShowObject> GetWatchesHotShowhome()
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> WatchHot = db.Watches.Where(a => a.Hot && a.Status && a.Quality > 0)
                       .Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID,(a, b) => new ShowObject
                           { 
                                ID = a.ID,
                                Alias = a.Alias,
                                Name = a.Name,
                                
                                TypeWatch = b.Titel,
                                Quality = a.Quality,
                                Image = a.Image,
                                Price = a.Price,
                                PriceDiscount = a.PriceDiscount,
                                Percent =a.Percent,
                                MenuAlias = a.Categary.Alias,
                            }).OrderByDescending(a => a.ID).ToList();
            return WatchHot;
            }
        }

        //Danh sách đồng hồ giảm giá
        public static List<ShowObject> GetWatchesFlash()
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> WatchFlash = db.Watches.Where(a => a.Percent > 0 && a.Status && a.Quality > 0)
                       .Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID, (a, b) => new ShowObject
                       {
                           ID = a.ID,
                           Alias = a.Alias,
                           Name = a.Name,

                           TypeWatch = b.Titel,
                           Quality = a.Quality,
                           Image = a.Image,
                           Price = a.Price,
                           PriceDiscount = a.PriceDiscount,
                           Percent = a.Percent,
                           MenuAlias = a.Categary.Alias,
                       }).OrderByDescending(a => a.ID).ToList();
                return WatchFlash;
            }
        }

        

        //Lấy đồng hồ 
        public static List<ShowObject> GetAllWatches()
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> WatchFlash = db.Watches.Where(a => a.Status && a.Quality > 0)
                       .Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID, (a, b) => new ShowObject
                       {
                           ID = a.ID,
                           Alias = a.Alias,
                           Name = a.Name,
                           Index = a.Index,
                           TypeWatch = b.Titel,
                           Quality = a.Quality,
                           Image = a.Image,
                           Price = a.Price,
                           PriceDiscount = a.PriceDiscount,
                           Percent = a.Percent,
                           CategaryID = a.CatageryID,
                           MenuAlias = a.Categary.Alias,
                       }).OrderByDescending(a => a.Index).ToList();
                return WatchFlash;
            }
        }

       

        //Lấy đồng hồ theo type 
        public static List<ShowObject> GetAllWatchesType(int id)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> WatchFlash = db.Watches.Where(a => a.Status && a.Quality > 0 && a.TypeID == id)
                       .Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID, (a, b) => new ShowObject
                       {
                           ID = a.ID,
                           Alias = a.Alias,
                           Name = a.Name,
                           Index = a.Index,
                           TypeWatch = b.Titel,
                           Quality = a.Quality,
                           Image = a.Image,
                           Price = a.Price,
                           PriceDiscount = a.PriceDiscount,
                           Percent = a.Percent,
                           CategaryID = a.CatageryID,
                           MenuAlias = a.Categary.Alias,
                       }).OrderByDescending(a => a.Index).ToList();
                return WatchFlash;
            }
        }

        //Danh sách đồng hồ theo categary và type
        public static List<ShowObject> WatchCategary(int categary,int id)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> WatchCategary = db.Watches.Where(a => a.Status && a.Categary.ID == categary && a.Quality > 0 && a.TypeID == id)
                       .Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID, (a, b) => new ShowObject
                       {
                           ID = a.ID,
                           Alias = a.Alias,
                           Name = a.Name,
                           Index = a.Index,
                           TypeWatch = b.Titel,
                           Quality = a.Quality,
                           Image = a.Image,
                           Price = a.Price,
                           PriceDiscount = a.PriceDiscount,
                           Percent = a.Percent,
                           CategaryID = a.CatageryID,
                           MenuAlias = a.Categary.Alias,
                       }).OrderByDescending(a => a.Index).ToList();
                return WatchCategary;
            }
        }
        //Lấy menu đồng hồ
        public static Menu MenuWatch()
        {
            using (var db = new MyDbDataContext())
            {
                Menu menuWatch = db.Menus.Where(a => a.Type == 2 && a.Status).FirstOrDefault();
                return menuWatch;
            }
        }

        //Chi tiết đồng hồ
       

        //Danh sách đồng hồ theo categary
        public static List<ShowObject> WatchCategary(int id)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> WatchCategary = db.Watches.Where(a => a.Status && a.CatageryID == id && a.Quality > 0)
                       .Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID, (a, b) => new ShowObject
                       {
                           ID = a.ID,
                           Alias = a.Alias,
                           Name = a.Name,
                           Index = a.Index,
                           TypeWatch = b.Titel,
                           Quality = a.Quality,
                           Image = a.Image,
                           Price = a.Price,
                           PriceDiscount = a.PriceDiscount,
                           Percent = a.Percent,
                           CategaryID = a.CatageryID,
                           MenuAlias = a.Categary.Alias,
                       }).OrderByDescending(a => a.Index).ToList();
                return WatchCategary;
            }
        }

        //Chi tiết đồng hồ
        public static DetailWatch detailWatch(int id)
        {
            using(var db = new MyDbDataContext())
            {
                Watch watch = db.Watches.FirstOrDefault(a => a.ID == id && a.Status) ?? new Watch();
                List<WatchGallery> watchGalleries = db.WatchGalleries.Where(a => a.WatchId == watch.ID).ToList();
                List<ShowObject> watches = db.Watches.Where(a => a.Status && a.ID != watch.ID && a.CatageryID == watch.Categary.ID).Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID, (a, b) => new ShowObject
                {
                    ID = a.ID,
                    Alias = a.Alias,
                    Name = a.Name,
                    Index = a.Index,
                    TypeWatch = b.Titel,
                    Quality = a.Quality,
                    Image = a.Image,
                    Price = a.Price,
                    PriceDiscount = a.PriceDiscount,
                    Percent = a.Percent,
                    CategaryID = a.CatageryID,
                    MenuAlias = a.Categary.Alias,
                    

                }).OrderBy(a => a.Index).ToList();

               
                DetailWatch detailWatch = new DetailWatch()
                {
                    watch = watch,
                    watchGalleries = watchGalleries,
                    watches = watches,
                    MenuAlias = watch.Categary.Alias,
                    TypeWatch = watch.TypeWatch.Titel,
                };
                return detailWatch;
            }
        }

        //Lấy đồng hồ giảm giá
        public static List<ShowObject> GetSaleWatches()
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> WatchFlash = db.Watches.Where(a => a.Status && a.Percent > 0 && a.Quality > 0)
                       .Join(db.TypeWatches.Where(a => a.Status), a => a.TypeID, b => b.ID, (a, b) => new ShowObject
                       {
                           ID = a.ID,
                           Alias = a.Alias,
                           Name = a.Name,
                           Index = a.Index,
                           TypeWatch = b.Titel,
                           Quality = a.Quality,
                           Image = a.Image,
                           Price = a.Price,
                           PriceDiscount = a.PriceDiscount,
                           Percent = a.Percent,
                           CategaryID = a.CatageryID,
                           MenuAlias = a.Categary.Alias,
                       }).OrderByDescending(a => a.Index).ToList();
                return WatchFlash;
            }
        }

       //Lấy bài viết hot
       public static List<Article> ArticlesHot()
        {
            using(var db = new MyDbDataContext())
            {
                List<Article> articles = db.Articles.Where(a => a.Status && a.Hot).ToList();
                return articles;
            }
        }

        //Lấy bài viết new
        public static List<Article> ArticlesNew()
        {
            using (var db = new MyDbDataContext())
            {
                List<Article> articles = db.Articles.Where(a => a.Status && a.New).ToList();
                return articles;
            }
        }

        //Lấy bài viết 
        public static List<Article> Articles()
        {
            using (var db = new MyDbDataContext())
            {
                List<Article> articles = db.Articles.Where(a => a.Status).ToList();
                return articles;
            }
        }
        //Chi tiết bài viết
        public static DetailArticle Detail_Article(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Article article = db.Articles.FirstOrDefault(a => a.ID == id && a.Status) ?? new Article();
                List<Article> articles = db.Articles.Where(a => a.Status && a.ID != article.ID).OrderBy(a => a.Index).Take(5).ToList();
              
                DetailArticle detailArticle = new DetailArticle()
                {
                    Article = article,
                    Articles = articles,
                    AliasMenu = "tin-tuc",
                };
                return detailArticle;
            }
        }

        //Chi tiết khách hàng
        public static List<Customer> GetCustomer()
        {
            using(var db = new MyDbDataContext())
            {
                List<Customer> customer = db.Customers.Where(a => a.Status).ToList();
                return customer;
            }
        }
    }
}