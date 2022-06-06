using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class ArticleController : BaseController
    {
        // GET: Administrator/Article
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang bài viết";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
              
                                   List < Article > records = db.Articles.ToList();

                foreach (var record in records)
                {
                    string itemAdv = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemAdv, out index);
                    record.Index = index;
                    db.SubmitChanges();
                }
                TempData["Messages"] = "Sắp xếp thành công";
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
                    List<Article> list = db.Articles.ToList();
                    var records = list.Select(a => new
                    {
                        a.ID,
                        a.Title,
                       a.Index,
                        a.Status,
                        a.Hot,
                        a.New
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
            ViewBag.Title = "Thêm bài viết";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EArticle model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Alias))
                    {
                        model.Alias = StringHelper.ConvertToAlias(model.Alias);
                    }
                    try
                    {
                        var article = new Article
                        {
                            Title = model.Title,
                            Alias = model.Alias,
                            Description = model.Description,
                            Content = model.Content,
                            
                            Image = model.Image,
                            Author = model.Author,
                            CreateDate = DateTime.Now,

                            Index = 0,
                            Status = model.Status,
                            Hot = model.Hot,
                            New = model.New,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.Description,
                           
                        };

                        db.Articles.InsertOnSubmit(article);
                        db.SubmitChanges();

                        TempData["Messages"] = "Thêm bài viết thành công";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View();
                    }
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.Title = "Cập nhật bài viết";
            using (var db = new MyDbDataContext())
            {
                Article detailArticle = db.Articles.FirstOrDefault(a => a.ID == id);

                if (detailArticle == null)
                {
                    TempData["Messages"] = "Bài viết không tồn tại";
                    return RedirectToAction("Index");
                }

                var artile = new EArticle
                {
                    ID = detailArticle.ID,
                    Title = detailArticle.Title,
                    Alias = detailArticle.Alias,
                    Image = detailArticle.Image,
                    Description = detailArticle.Description,
                    Content = detailArticle.Content,
                    MetaTitle = detailArticle.MetaTitle,
                    MetaDescription = detailArticle.MetaDescription,
                    Status = detailArticle.Status,
                    Hot = detailArticle.Hot,
                    New = detailArticle.New,
                    Author = detailArticle.Author,
                    DateCreate = (DateTime)detailArticle.CreateDate,
                };
                return View(artile);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EArticle model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Alias))
                    {
                        model.Alias = StringHelper.ConvertToAlias(model.Alias);
                    }
                    try
                    {
                        Article article = db.Articles.FirstOrDefault(b => b.ID == model.ID);
                        if (article != null)
                        {
                            article.Title = model.Title;
                            article.Alias = model.Alias;
                            article.Image = model.Image;
                            article.Description = model.Description;
                            article.Content = model.Content;
                            article.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle;
                            article.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                                ? model.Title
                                : model.MetaDescription;
                            article.Status = model.Status;
                            article.Hot = model.Hot;
                            article.New = model.New;
                            article.Author = model.Author;
                            article.CreateDate = DateTime.Now;

                            db.SubmitChanges();
                            TempData["Messages"] = "Cập nhật bài viết thành công";
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
                    Article del = db.Articles.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Articles.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa bài viết thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Bài viết không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        
    }
}