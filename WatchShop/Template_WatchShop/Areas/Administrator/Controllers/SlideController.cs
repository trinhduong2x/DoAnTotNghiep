using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.ModelShow;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class SlideController : BaseController
    {
        // GET: Administrator/Slide
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang slide ảnh";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
                List<Slide> records =
                    db.Slides.ToList();
                foreach (Slide record in records)
                {
                    string itemAdv = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemAdv, out index);
                    record.Index = index;
                    db.SubmitChanges();
                }
                TempData["Messages"] = "Sắp xếp slide thành công";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Slide> listSlider =
                        db.Slides.ToList();
                    List<ShowSlider> records = listSlider.Select(a => new ShowSlider
                    {
                        ID = a.ID,
                        Title = a.Title,
                        Image = a.Image,
                       
                        Status = a.Status,
                        Index = a.Index,
                    }).OrderBy(c => c.Index).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listSlider.Count });
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
            var model = new ESlide();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ESlide model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                      
                        var slider = new Slide
                        {
                          
                            Title = model.Title,
                            Image = model.Image,
                            Index = 0,
                            Status = model.Status,
                        };

                        db.Slides.InsertOnSubmit(slider);
                        db.SubmitChanges();

                        TempData["Messages"] = "Thêm mới slide thành công";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                      
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
               
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Slide detailSlider = db.Slides.FirstOrDefault(a => a.ID == id);
                if (detailSlider == null)
                {
                    TempData["Messages"] = "Slide không tồn tại";
                    return RedirectToAction("Index");
                }
                var slider = new ESlide
                {
                    ID = detailSlider.ID,
                    Title = detailSlider.Title,
                    Image = detailSlider.Image,
                    Status = detailSlider.Status,
                };
                ViewBag.Title = "Sửa slide";
                return View(slider);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ESlide model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        Slide slider = db.Slides.FirstOrDefault(b => b.ID == model.ID);
                      
                        if (slider != null)
                        {
                            slider.Title = model.Title;
                            slider.Image = model.Image;
                            slider.Status = model.Status;
                            db.SubmitChanges();
                            TempData["Messages"] = "Sửa slide thành công.";
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception exception)
                {
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Slide del = db.Slides.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Slides.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa slide thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Slide không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        //Lấy danh sách menu khi thay đổi hotel
        //public List<MenuCheck> LoadData(string menuIDs, string languageID)
        //{
        //    //check logged
        //    var menuIsSelect = new List<MenuCheck>();
        //    List<Menu> listMenu =
        //        MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId, languageID);


        //    var listMenuIds = new int[1];
        //    if (string.IsNullOrEmpty(menuIDs) == false)
        //    {
        //        listMenuIds =
        //            menuIDs.Substring(0, menuIDs.Length - 1)
        //                .Split(',')
        //                .Select(n => Convert.ToInt32(n))
        //                .ToArray();
        //    }
        //    menuIsSelect =
        //        listMenu.Select(a => new MenuCheck
        //        {
        //            Checked = listMenuIds.Contains(a.ID) ? "checked" : "",
        //            Level = a.Level,
        //            ID = a.ID,
        //            Title = a.Title
        //        }).ToList();
        //    return menuIsSelect;
        //}
        //Lấy danh sách menu chan trang
        //public List<MenuCheck> LoadData1(string menuIDs, string languageID)
        //{
        //    //check logged
        //    var menuIsSelect = new List<MenuCheck>();

        //    List<Menu> listMenu =
        //        MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[1].LocationId, languageID);

        //    var listMenuIds = new int[1];
        //    if (string.IsNullOrEmpty(menuIDs) == false)
        //    {
        //        listMenuIds =
        //            menuIDs.Substring(0, menuIDs.Length - 1)
        //                .Split(',')
        //                .Select(n => Convert.ToInt32(n))
        //                .ToArray();
        //    }
        //    menuIsSelect =
        //        listMenu.Select(a => new MenuCheck
        //        {
        //            Checked = listMenuIds.Contains(a.ID) ? "checked" : "",
        //            Level = a.Level,
        //            ID = a.ID,
        //            Title = a.Title
        //        }).ToList();
        //    return menuIsSelect;
        //}

    }
}