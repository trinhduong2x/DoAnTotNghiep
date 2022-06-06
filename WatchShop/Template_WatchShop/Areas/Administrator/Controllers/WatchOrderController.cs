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
    public class WatchOrderController : Controller
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