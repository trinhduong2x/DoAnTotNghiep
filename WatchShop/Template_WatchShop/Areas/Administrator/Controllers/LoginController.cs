using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.Models;
using ProjectLibrary.Security;
using Template_WatchShop.Areas.Administrator.EntityModel;
using ProjectLibrary.Database;
using ProjectLibrary.Config;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class LoginController : Controller
    {
      [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["name_client"] != null)
            {
                return RedirectToAction("Index", "ControlPanel");
            }
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Index(LoginModel model)
        {
            Random random;
            if (TempData["Count"] != null && int.Parse(TempData["Count"].ToString()) >= 5)
            {
                TempData.Keep("Count");
                if (string.IsNullOrEmpty(model.Captcha))
                {
                    ViewBag.ShowCaptcha = true;
                    ModelState.AddModelError("Captcha", "Vui lòng nhập captcha");
                }
                else
                {
                    if (model.Captcha != Session["Captcha"].ToString())
                    {
                        ViewBag.ShowCaptcha = true;
                        ModelState.AddModelError("Captcha", "Mã captcha không đúng");
                    }
                }
            }
                if (ModelState.IsValid)
                {
                    int checklogin = CheckLogin.CheckUserLogin(model.UserName, model.Password);
                    switch (checklogin)
                    {
                        case 1:
                            //Đăng nhập thành công
                            string cookieClient = model.UserName + "||";
                            string decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                            HttpCookie userCookie = new HttpCookie("name_client");
                            userCookie.Value = decodeCookieClient;
                            userCookie.Expires = DateTime.Now.AddDays(30);
                            HttpContext.Response.Cookies.Add(userCookie);
                            TempData["Count"] = 0;
                            TempData["Messages"] = "Đăng nhập thành công";
                            return RedirectToAction("Index", "ControlPanel");
                        case 2:
                            TempData["Messages"] = "Tài khoản đã bị khóa";
                            return RedirectToAction("Index");
                        case 3:
                            if (TempData["Count"] == null)
                            {
                                TempData["Count"] = 1;
                                TempData.Keep("Count");
                            }
                            else
                            {
                                TempData["Count"] = int.Parse(TempData["Count"].ToString()) + 1;
                                TempData.Keep("Count");
                            }
                            if (int.Parse(TempData["Count"].ToString()) >= 5)
                            {
                                random = new Random();
                                int iNumber = random.Next(10000, 99999);
                                Session["Captcha"] = iNumber.ToString();

                                ViewBag.ShowCaptcha = true;
                                ViewBag.Message = CommentController.Messages(TempData["Message"]);
                                return View();
                            }
                     
                        ViewBag.Messages = "Tên đăng nhập và mật khẩu không đúng";
                            return View(model);
                    }
                }
                return View();
            
        }
        [HttpGet]
        [ValidateInput(true)]
        public ActionResult GetPassWord()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult GetPassWord(GetPassword model)
        {
            using (var db = new MyDbDataContext())
            {
                User detailUser =
                   db.Users.FirstOrDefault(a => a.Email == model.Email && a.UserName == model.Username);
                if (Convert.ToDateTime(CurrentSession.LockUser) > DateTime.Now)
                {
                    DateTime dateBlog = Convert.ToDateTime(CurrentSession.LockUser);
                    int minuteLock = dateBlog.Minute + (dateBlog.Hour * 60) - DateTime.Now.Minute - (DateTime.Now.Hour * 60);
                    ModelState.AddModelError("Email",
                        "Bạn đã nhập quá nhiều lần quy định, xin vui lòng quay lại sau " + minuteLock + " Phút.");
                    return View();
                }
                if (detailUser == null)
                {
                    if (TempData["Count"] == null)
                    {
                        TempData["Count"] = 1;
                        TempData.Keep("Count");
                    }
                    else
                    {
                        TempData["Count"] = int.Parse(TempData["Count"].ToString()) + 1;
                        TempData.Keep("Count");
                    }
                    if (int.Parse(TempData["Count"].ToString()) == 5)
                    {
                        DateTime dateBlog = DateTime.Now.AddMinutes(1);
                        CurrentSession.LockUser = dateBlog;
                        int minuteLock = dateBlog.Minute + (dateBlog.Hour * 60) - DateTime.Now.Minute -
                                         (DateTime.Now.Hour * 60);
                        TempData.Remove("Count");
                        ModelState.AddModelError("Email",
                            "Bạn đã nhập quá nhiều lần quy định, xin vui lòng quay lại sau " + minuteLock + " phút.");
                        return View();
                    }
                    ModelState.AddModelError("Email",
                        "Email hoặc tên người dùng là không chính xác, bạn còn " +
                        (5 - int.Parse(TempData["Count"].ToString())) + " lần nhập");

                    return View();
                }
                string content =
                    System.IO.File.ReadAllText(Server.MapPath("/Areas/Administrator/Content/Teamplate/Forgot_Password.html"));
                content = content.Replace("{{Password}}", CryptorEngine.Decrypt(detailUser.PasswordOld, true));

                MailHelper.SendMail(detailUser.Email, "Lấy lại mật khẩu", content);


                ViewBag.Messeages = "Vui lòng đăng nhập vào địa chỉ email: " + model.Email + " để lấy lại mật khẩu.";
                return View();
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            int cout = 0;
            HttpCookie nameCookie = Request.Cookies["name_client"];
            while (nameCookie != null)
            {
                nameCookie.Expires = DateTime.Now.AddDays(-30);
                HttpContext.Response.Cookies.Add(nameCookie);
                cout++;
                if (cout == 10)
                    break;
            }
            CurrentSession.ClearAll();
            return RedirectToAction("Index");
        }
    }
}