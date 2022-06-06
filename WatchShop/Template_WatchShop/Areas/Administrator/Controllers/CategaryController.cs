using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class CategaryController : BaseController
    {
        // GET: Administrator/Categary
       
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang hãng đồng hồ";
            return View();
        }   

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                  var listcate = db.Categaries.Select(a => new                      
                        {
                            a.ID,
                            a.Titel,                        
                            a.Image,                                                 
                                                     
                        }).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = listcate, TotalRecordCount = listcate.Count });
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
          
            ViewBag.Title = "Thêm hãng đồng hồ";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ECategary model)
        {
            using (var db = new MyDbDataContext())
            {
               
                if (ModelState.IsValid)
                {
                    try
                    {
                        var categary = new Categary
                        {
                            Titel = model.Title,
                            Alias = model.Alias,
                           
                            Image = model.Image,
                           Status = model.Status,
                        };

                        db.Categaries.InsertOnSubmit(categary);
                        db.SubmitChanges();

                        TempData["Messages"] = "Thêm hãng thành công";
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
            ViewBag.Title = "Cập nhật hãng đồng hồ";
            using (var db = new MyDbDataContext())
            {
                Categary categary = db.Categaries.FirstOrDefault(a => a.ID == id);

                if (categary == null)
                {
                    TempData["Messages"] = "Hãng không tồn tại";
                    return RedirectToAction("Index");
                }

                var eCategary = new ECategary
                {
                    Title = categary.Titel,
                    Alias = categary.Alias,
                    Image = categary.Image,
                   
                    Status = categary.Status,
                };
                return View(eCategary);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ECategary model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Categary categary = db.Categaries.FirstOrDefault(b => b.ID == model.ID);
                        if (categary != null)
                        {
                            categary.Titel = model.Title;
                            categary.Image = model.Image;
                            categary.Alias = model.Alias;
                            categary.Status = model.Status;
                           

                            db.SubmitChanges();
                            TempData["Messages"] = "Cập nhật  thành công";
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
                    Categary del = db.Categaries.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Categaries.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa  thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = " không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public static List<Categary> GetListCategary()
        {
            using (var db = new MyDbDataContext())
            {
                List<Categary> categaries = db.Categaries.Where(a => a.Status).ToList();
                return categaries;
            }

        }
    }
}