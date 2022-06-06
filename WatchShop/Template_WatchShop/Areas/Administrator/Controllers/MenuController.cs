using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using Template_WatchShop.Areas.Administrator.EntityModel;
using Template_WatchShop.Areas.Administrator.ModelShow;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class MenuController : BaseController
    {
        [HttpGet]
        public ActionResult MainMenu()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.MenuLocation = GetLocaltion(SystemMenuLocation.MainMenu);
            return View("Index");
        }

        [HttpGet]
        public ActionResult SecondMenu()
        {
            if (TempData["Messages"] != null)
            {
                ViewBag.Messages = TempData["Messages"];
            }
            ViewBag.MenuLocation = GetLocaltion(SystemMenuLocation.SecondMenu);
            return View("Index");
        }

        [HttpPost]
        public JsonResult List(int locationID = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            using (var db = new MyDbDataContext())
            {


                try
                {
                    SystemMenuLocation locationMenu = GetLocaltion(locationID);
                    List<EMenu> listMenuShow = db.Menus.Where(a => a.Location == locationMenu.LocationId && a.Status).Select(a => new EMenu
                    {
                        Index = a.Index,
                        ID = a.ID,
                        Status = a.Status,
                        Title = a.Title,
                        Type = a.Type,
                    }).ToList();


                    return Json(new { Result = "OK", Records = listMenuShow, TotalRecordCount = listMenuShow.Count });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", message = ex.Message });
                }
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ListData();
            SystemMenuLocation systemMenuLocation = GetLocaltion(0);
            ViewBag.MenuLocation = systemMenuLocation;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EMenu model)
        {
            if (ModelState.IsValid)
            {
                SystemMenuLocation menuLocation = GetLocaltion(model.Location);
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        if (string.IsNullOrEmpty(model.Alias))
                        {
                            model.Alias = StringHelper.ConvertToAlias(model.Title);
                        }
                        //Kiểm tra xem alias thuộc hotel này đã tồn tại chưa
                        Menu checkMenuAlias =
                            db.Menus.FirstOrDefault(m => m.Alias == model.Alias);
                        if (checkMenuAlias != null)
                        {
                            ModelState.AddModelError("Alias", "Menu này đã tồn tại trong hệ thống");
                        }

                        if (model.Type == SystemMenuType.Home)
                        {
                            model.Alias = "";
                        }

                        var menu = new Menu
                        {
                            Alias = model.Alias,
                            Index = 0,
                            Location = menuLocation.LocationId,
                            MetaDescription = string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.MetaDescription,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle,
                            Status = model.Status,
                            Title = model.Title,
                            Type = model.Type,
                            //Image = model.Image,
                            Description = string.IsNullOrEmpty(model.Description) ? model.Title : model.Description,
                        };
                        db.Menus.InsertOnSubmit(menu);
                        db.SubmitChanges();
                        TempData["Messages"] = "Thêm chuyên mục thành công";
                        return RedirectToAction(menuLocation.AliasMenu);
                    }

                }
                catch (Exception exception)
                {
                    ListData();
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View();
                }
            }
            ListData();
            return View();
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Menu menu = db.Menus.FirstOrDefault(m => m.ID == id);

                SystemMenuLocation menuLocation = GetLocaltion(0);
                if (menu != null)
                {
                    var model = new EMenu
                    {
                        Alias = menu.Alias,
                        Index = menu.Index,
                        Location = menu.Location,
                        ID = menu.ID,
                        MetaDescription = menu.MetaDescription,
                        MetaTitle = menu.MetaTitle,
                        Status = menu.Status,
                        Title = menu.Title,
                        Type = menu.Type,
                        //Image = menu.Image,
                        Description = menu.Description,
                    };
                    ListData();
                    ViewBag.MenuLocation = GetLocaltion(0);
                    return View(model);
                }
                TempData["Messages"] = "Chuyên mục không tồn tại";
                return RedirectToAction(menuLocation.AliasMenu);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EMenu model)
        {
            //Kiểm tra xem alias thuộc hotel này đã tồn tại chưa
            var db = new MyDbDataContext();
            Menu checkMenuAlias = db.Menus.FirstOrDefault(m => m.Alias == model.Alias && m.ID != model.ID);
            if (checkMenuAlias != null)
            {
                ModelState.AddModelError("Alias", "Chuyên mục này đã tồn tại trong hệ thống");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    Menu edit = db.Menus.FirstOrDefault(m => m.ID == model.ID);

                    if (edit != null)
                    {
                        model.Alias = !string.IsNullOrEmpty(model.Alias) ? model.Alias : StringHelper.ConvertToAlias(model.Title);
                        SystemMenuLocation menuLocation = GetLocaltion(edit.Location);
                        if (model.Type == SystemMenuType.Home)
                        {
                            model.Alias = "";
                        }

                        edit.Alias = model.Alias;
                        edit.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle;
                        edit.Title = model.Title;
                        edit.Type = model.Type;
                        edit.Status = model.Status;
                        edit.MetaDescription = string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.MetaDescription;
                        edit.Description = string.IsNullOrEmpty(model.Description) ? model.Title : model.Description;
                        db.SubmitChanges();



                        TempData["Messages"] = "Sửa chuyên mục thành công";
                        return RedirectToAction(menuLocation.AliasMenu);
                    }
                    SystemMenuLocation menulocation2 = GetLocaltion(0);
                    TempData["Messages"] = "Chuyên mục không tồn tại";
                    return RedirectToAction(menulocation2.AliasMenu);
                }
                catch (Exception exception)
                {
                    ListData();
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View(model);
                }
            }
            ListData();
            ViewBag.Messages = "Dữ liệu đầu vào không đúng định dạng";
            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Menu del = db.Menus.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {

                        db.Menus.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa chuyên mục thành công" });
                    }
                    return Json(new { Result = "OK", Message = "Chuyên mục này không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateIndex(int locationID)
        {
            SystemMenuLocation localtionMenu = GetLocaltion(locationID);
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Menu> menus =
                        db.Menus.ToList();

                    foreach (Menu item in menus)
                    {
                        string requestIndex = Request.Params[string.Format("Sort[{0}].Index", item.ID)];
                        int index;
                        int.TryParse(requestIndex, out index);
                        Menu temp = db.Menus.FirstOrDefault(c => c.ID == item.ID);
                        if (temp != null)
                        {
                            temp.Index = index;
                            db.SubmitChanges();
                        }
                    }
                }
                TempData["Messages"] = "Sắp xếp chuyên mục thành công";
                return RedirectToAction(localtionMenu.AliasMenu, "Menu");
            }
            catch (Exception ex)
            {
                TempData["Messages"] = "Error: " + ex.Message;
                return RedirectToAction(localtionMenu.AliasMenu, "Menu");
            }
        }

        public SystemMenuLocation GetLocaltion(int locationId)
        {
            SystemMenuLocation menuLocation =
                SystemMenuLocation.ListLocationMenu().ToList().FirstOrDefault(m => m.LocationId == locationId);
            if (menuLocation != null)
            {
                return menuLocation;
            }
            string aliasMenu = Request.QueryString["location"];
            if (string.IsNullOrEmpty(aliasMenu) == false)
            {
                return SystemMenuLocation.ListLocationMenu().ToList().FirstOrDefault(m => m.AliasMenu == aliasMenu);
            }
            return new SystemMenuLocation { AliasMenu = "MainMenu", TitleMenu = "chuyên mục chính", LocationId = 1 };
        }

        //lấy danh sách menu theo ngôn ngữ, theo hotel, theo vị trí, theo AllHotel


        //Lấy danh sách typemenu, parentMene
        public void ListData()
        {
            var listTypeMenu = new List<SelectListItem>();
            listTypeMenu.Add(new SelectListItem
            {
                Text = "Chọn kiểu hiển thị",
                Value = "0",
            });
            listTypeMenu.AddRange(SystemMenuType.CategoryType.Select(a => new SelectListItem
            {
                Text = a.Value,
                Value = a.Key.ToString(CultureInfo.InvariantCulture)
            }).ToList());
            ViewBag.ListTypeMenu = listTypeMenu;
        }
    }
}