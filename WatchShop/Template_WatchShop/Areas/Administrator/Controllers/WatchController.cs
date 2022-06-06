using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using Rotativa;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class WatchController : Controller
    {
        // GET: Administrator/Watch
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang quản lý đồng hồ";
            ListCategary();
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
                List<Watch> records = db.Watches.ToList();
                foreach (Watch record in records)
                {
                    string itemRoom = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemRoom, out index);
                    record.Index = index;
                    db.SubmitChanges();
                }
                TempData["Messages"] = "Sắp xếp đồng hồ thành công";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult List(string search,int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    int seach = Convert.ToInt32(search);
                    List<Watch> list = db.Watches.ToList();
                    if(seach != 0)
                    {
                        list = list.Where(a => a.CatageryID == seach).ToList();
                    }    
                    var records = list.Select(a => new
                    {
                        a.ID,
                        a.Name,
                        a.Index,
                        a.Quality,
                        a.Price,                      
                      a.Image,
                        a.New,
                        a.Status,
                        a.Percent,
                        
                    }).OrderBy(a => a.Index).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = list.Count() });
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
            LoadData();
            ViewBag.Title = "Thêm đồng hồ";
            var eWatch = new EWatch();
            return View(eWatch);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EWatch model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Alias))
                    {
                        model.Alias = StringHelper.ConvertToAlias(model.Name);
                    }
                    Watch checkAlias =
                           db.Watches.FirstOrDefault(m => m.Alias == model.Alias);
                    if (checkAlias != null)
                    {
                        ModelState.AddModelError("Alias", "Watch này đã tồn tại trong hệ thống");
                    }
                    try
                    {
                        var watch = new Watch
                        {
                            Name = model.Name,
                            Alias = model.Alias,
                            Image = model.Image,
                            Price = model.Price,
                            PriceDiscount = model.PriceDiscount,
                            Index = 0,
                            Hot = model.Hot,
                            New = model.New,
                            Description = model.Description,
                            FaceDiameter = model.FaceDiameter,
                            FaceMaterial = model.FaceMaterial,
                            EnergyUse = model.EnergyUse,
                            WaterProof = model.WaterProof,
                            Size = model.Size,
                            StringMaterial = model.StringMaterial,
                            MadeIn = model.MadeIn,
                            Insurance = model.Insurance,
                            Percent = model.Percent,
                            Quality = model.Quality,
                            CatageryID = model.CatageryID,
                            TypeID = model.TypeID,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Name : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Name : model.MetaDescription,
                            Status = model.Status,
                          
                        };

                        db.Watches.InsertOnSubmit(watch);
                        db.SubmitChanges();

                        //Thêm hình ảnh cho phòng
                        if (model.EGalleryITems != null)
                        {
                            foreach (EGalleryITem itemGallery in model.EGalleryITems)
                            {
                                var watchGallery = new WatchGallery
                                {
                                    ImageLarge = itemGallery.Image,
                                    ImageSmall = ReturnSmallImage.GetImageSmall(itemGallery.Image),
                                    WatchId = watch.ID,
                                };
                                db.WatchGalleries.InsertOnSubmit(watchGallery);
                            }
                            db.SubmitChanges();
                        }

                        TempData["Messages"] = "Thêm đồng hồ thành công.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        LoadData();

                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                LoadData();

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Watch watch = db.Watches.FirstOrDefault(a => a.ID == id);
                if (watch == null)
                {
                    TempData["Messages"] = "Đồng hồ không tồn tại";
                    return RedirectToAction("Index");
                }
                ViewBag.Title = "Sửa đồng hồ";
                var watch1 = new EWatch
                {
                    ID = watch.ID,                   
                    Name = watch.Name,
                    Alias = watch.Alias,
                    Image = watch.Image,
                    Price = watch.Price,
                    PriceDiscount = watch.PriceDiscount,
                    Hot = watch.Hot,
                    New = watch.New,
                    Description = watch.Description,
                    FaceDiameter = watch.FaceDiameter,
                    FaceMaterial = watch.FaceMaterial,
                    EnergyUse = watch.EnergyUse,
                    WaterProof = watch.WaterProof,
                    Size = watch.Size,
                    StringMaterial = watch.StringMaterial,
                    MadeIn = watch.MadeIn,
                    Insurance = watch.Insurance,
                    Percent =watch.Percent,
                    Quality = watch.Quality,
                    CatageryID = watch.CatageryID,
                    TypeID = watch.TypeID,
                    MetaTitle = watch.MetaTitle,
                    MetaDescription =
                                 watch.MetaTitle,
                    Status = watch.Status,
                };
                //lấy danh sách hình ảnh
                watch1.EGalleryITems = db.WatchGalleries.Where(a => a.WatchId == watch.ID).Select(a => new EGalleryITem
                {
                    Image = a.ImageLarge
                }).ToList();
                LoadData();
                return View(watch1);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EWatch model)
        {
            using (var db = new MyDbDataContext())
            {
                Watch checkAlias = db.Watches.FirstOrDefault(m => m.Alias == model.Alias && m.ID != model.ID);
                if (checkAlias != null)
                {
                    ModelState.AddModelError("Alias", "Chuyên mục này đã tồn tại trong hệ thống");
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        Watch watch = db.Watches.FirstOrDefault(b => b.ID == model.ID);
                        if (watch != null)
                        {

                            watch.Name = model.Name;
                            watch.Alias = model.Alias;
                            watch.Image = model.Image;
                            watch.Price = model.Price;
                            watch.PriceDiscount = model.PriceDiscount;
                            watch.Hot = model.Hot;
                            watch.New = model.New;
                            watch.Description = model.Description;
                            watch.FaceDiameter = model.FaceDiameter;
                            watch.FaceMaterial = model.FaceMaterial;
                            watch.EnergyUse = model.EnergyUse;
                            watch.WaterProof = model.WaterProof;
                            watch.Size = model.Size;
                            watch.StringMaterial = model.StringMaterial;
                            watch.MadeIn = model.MadeIn;
                            watch.Insurance = model.Insurance;
                            watch.Percent = model.Percent;
                            watch.Quality = model.Quality;
                            watch.CatageryID = model.CatageryID;
                            watch.TypeID = model.TypeID;
                            watch.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Name : model.MetaTitle;
                            watch.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                                ? model.Name
                                : model.MetaDescription;
                            watch.Status = model.Status;
                            db.SubmitChanges();

                            //xóa gallery 
                            db.WatchGalleries.DeleteAllOnSubmit(db.WatchGalleries.Where(a => a.WatchId == watch.ID).ToList());
                            //Thêm hình ảnh 
                            if (model.EGalleryITems != null)
                            {
                                foreach (EGalleryITem itemGallery in model.EGalleryITems)
                                {
                                    var watchGallery = new WatchGallery
                                    {
                                        ImageLarge = itemGallery.Image,
                                        ImageSmall = ReturnSmallImage.GetImageSmall(itemGallery.Image),
                                        WatchId = watch.ID,
                                    };
                                    db.WatchGalleries.InsertOnSubmit(watchGallery);
                                }
                                db.SubmitChanges();
                            }
                            LoadData();

                            TempData["Messages"] = "Sửa  thành công";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception exception)
                    {
                        LoadData();

                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                LoadData();
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
                    Watch del = db.Watches.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        //xóa hết hình ảnh của phòng này
                        db.WatchGalleries.DeleteAllOnSubmit(db.WatchGalleries.Where(a => a.WatchId == del.ID).ToList());

                        db.Watches.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa phòng thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Phòng không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public void LoadData()
        {
            var listType = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "Lựa chọn loại đồng hồ"}
            };
                var typeWatches = new List<TypeWatch>();
            typeWatches = TypeWatchController.GetListType().ToList();
            listType.AddRange(typeWatches.Select(a => new SelectListItem
            {
                Text = a.Titel,
                Value = a.ID.ToString()
            }).ToList());                           
            ViewBag.ListType = listType;
            //
            var listCategaries = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "Lựa chọn hãng đồng hồ"}
            };
            var categaries = new List<Categary>();
            categaries = CategaryController.GetListCategary().ToList();
            listCategaries.AddRange(categaries.Select(b => new SelectListItem
            {
                Text = b.Titel,
                Value = b.ID.ToString()
            }).ToList());
            ViewBag.ListCategaries = listCategaries;

        }

        public ActionResult WatchOver()
        {
            ViewBag.Title = "Export PDF";
            List<Watch> list = CommentController.GetWatches();
            if (list.Count > 0)
            {
                return View(list);
            }
            else
            {
                return View("Watch/View");
            }    
        }

        public ActionResult ExportPDF()
        {
            ActionAsPdf result = new ActionAsPdf("WatchOver")
            {
                FileName = Server.MapPath("~/Content/Invoice.pdf")
            };
            return result;
        }
        public void ListCategary()
        {
            var db = new MyDbDataContext();
            var listType = new List<SelectListItem>();
            listType.Add(new SelectListItem
            {
                Text = "Chọn thương hiệu",
                Value = "0",
            });
            listType.AddRange(db.Categaries.Select(a => new SelectListItem
            {
                Text = a.Titel,
                Value = a.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList());
            ViewBag.ListType = listType;
        }
    }
}