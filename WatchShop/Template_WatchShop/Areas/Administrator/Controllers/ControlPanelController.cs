﻿
using System.Web.Mvc;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class ControlPanelController : BaseController
    {
        // GET: Administrator/ControlPanel
        public ActionResult Index()
        {
            ViewBag.Title = "Trang quản trị";
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            return View();
        }

        public ActionResult Turnover()
        {
           
            return View();
        }
    }
}