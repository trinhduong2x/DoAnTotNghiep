using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class ShopController : BaseController
    {
        // GET: Administrator/Shop
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang Cửa hàng";
            return View();
        }
        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var list = db.Watchshops.Select(a => new
                    {
                        a.ID,
                        a.Name,
                        a.Logo,
                        a.Image,
                        a.Email,
                        a.Tel,
                        a.Address
                    }).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            using (var db = new MyDbDataContext())
            {
                var shop = db.Watchshops.ToList();
                if (shop.Count > 1 )
                {
                    TempData["Messages"] = "Website đã tồn tại cửa hàng rồi";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Title = "Thêm Khách sạn";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EShop model)
        {
            using (var db = new MyDbDataContext())
            {
                var shop = db.Watchshops.ToList();
                if (shop.Count > 0)
                {
                    TempData["Messages"] = "Website đã tồn tại khách sạn rồi";
                    return RedirectToAction("Index");
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        var watchshop = new Watchshop
                        {                           
                            Name = model.Name,
                            Address = model.Address,
                            Tel = model.Tel,
                            Email = model.Email,
                            Logo = model.Logo,
                            Image = model.Image,
                            Location = model.Location,
                            CodeBooking = model.CodeBooking,

                            Description = model.Description,
                            Facebook = model.FaceBook,
                            Zalo = model.Zalo,                                                                  
                            CopyRight = model.CopyRight,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Name : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Name : model.MetaDescription
                        };

                        db.Watchshops.InsertOnSubmit(watchshop);
                        db.SubmitChanges();

                        TempData["Messages"] = "Thêm cửa hàng thành công";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Lỗi: " + exception.Message;
                        return View();
                    }
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.Title = "Cập nhật cửa hàng";
            using (var db = new MyDbDataContext())
            {
                Watchshop watchshop = db.Watchshops.FirstOrDefault(a => a.ID == id);

                if (watchshop == null)
                {
                    TempData["Messages"] = "Nhà hàng không tồn tại";
                    return RedirectToAction("Index");
                }

                var eShop = new EShop
                {
                    Name = watchshop.Name,
                    Logo = watchshop.Logo,
                    Image = watchshop.Image,
                    Tel = watchshop.Tel,                  
                    Email = watchshop.Email,
                    Address = watchshop.Address,
                    Location = watchshop.Location,
                    CodeBooking = watchshop.CodeBooking,

                    Description = watchshop.Description,
                    FaceBook = watchshop.Facebook,
                    Zalo = watchshop.Zalo,                
                    CopyRight = watchshop.CopyRight,
                    MetaTitle = watchshop.MetaTitle,
                    MetaDescription = watchshop.MetaDescription
                };
                return View(eShop);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EShop model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Watchshop watchshop = db.Watchshops.FirstOrDefault(b => b.ID == model.ID);
                        if (watchshop != null)
                        {
                            watchshop.Name = model.Name;
                            watchshop.Logo = model.Logo;
                            watchshop.Image = model.Image;
                            watchshop.Tel = model.Tel;                          
                            watchshop.Email = model.Email;
                            watchshop.Address = model.Address;
                            watchshop.Location = model.Location;
                            watchshop.CodeBooking = model.CodeBooking;

                            watchshop.Description = model.Description;
                            watchshop.Facebook = model.FaceBook;
                            watchshop.Zalo = model.Zalo;
                            watchshop.CopyRight = model.CopyRight;
                            watchshop.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Name : model.MetaTitle;
                            watchshop.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                                ? model.Name
                                : model.MetaDescription;

                            db.SubmitChanges();
                            TempData["Messages"] = "Cập nhật cửa hàng thành công";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Lỗi: " + exception.Message;
                        return View();
                    }
                }
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Watchshop del = db.Watchshops.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Watchshops.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa cửa hàng thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Cửa hàng không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }

}