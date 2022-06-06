using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class TypeWatchController : BaseController
    {
        // GET: Administrator/TypeWatch
        public ActionResult Index()
        {
            ViewBag.Title = "Trang loại đồng hồ";
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            return View();
        }


        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<ETypeWatch> records = db.TypeWatches.Select(a => new ETypeWatch
                    {
                        ID = a.ID,
                        Title = a.Titel,                          
                        Status = a.Status,
                    }).ToList();

                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
                }

            }

            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Create(ETypeWatch model)
        {
            using (var db = new MyDbDataContext())
            {
                TypeWatch typeWatch = db.TypeWatches.FirstOrDefault(a => a.Titel == model.Title);
                if (typeWatch != null)
                {
                    ModelState.AddModelError("typeWatch",
                        "Tên này đã tồn tại");
                }
                
                if (ModelState.IsValid)
                {
                    try
                    {
                        var insert = new TypeWatch
                        {
                            Titel = model.Title,
                           
                            Status = model.Status
                        };

                        db.TypeWatches.InsertOnSubmit(insert);
                        db.SubmitChanges();
                        model.ID = insert.ID;

                        return Json(new { Result = "OK", Message = "Thêm  thành công", Record = model });
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return
                    Json(
                        new
                        {
                            Result = "Error",
                            Errors = ModelState.Errors(),
                            Message = "Dữ liệu đầu vào không đúng định dạng"
                        });
            }
        }

        [HttpPost]
        public ActionResult Edit(ETypeWatch model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    //check username đã tồn tại chưa
                    IQueryable<TypeWatch> typeWatches = db.TypeWatches.Where(a => a.Titel == model.Title);
                    if (typeWatches.Count() > 1)
                    {
                        ModelState.AddModelError("UserName",
                            "Tên đã tồn tại trong hệ thống");
                    }

                  

                    TypeWatch edit = db.TypeWatches.FirstOrDefault(b => b.ID == model.ID);
                    if (edit == null)
                    {
                        TempData["Messages"] = "Tên dùng không tồn tại trong hệ thống";
                        return RedirectToAction("Index");
                    }
                 
                    try
                    {
                        edit.Titel = model.Title;
                       
                        edit.Status = model.Status;

                      
                        db.SubmitChanges();

                        return Json(new { Result = "OK", Message = "Insert successful" });
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return
                    Json(
                        new
                        {
                            Result = "Error",
                            Errors = ModelState.Errors(),
                            Message = "Dữ liệu đầu vào không đúng định dạng"
                        });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    TypeWatch del = db.TypeWatches.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        //Xóa 
                        db.TypeWatches.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Không tồn tại trong hệ thống" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
            }
        }

        public static List<TypeWatch> GetListType()
        {
            using (var db = new MyDbDataContext())
            {
                List<TypeWatch> typeWatches = db.TypeWatches.Where(a => a.Status).ToList();
                return typeWatches;
            }
            
        }
    }
}