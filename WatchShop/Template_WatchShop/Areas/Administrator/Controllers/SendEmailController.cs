﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class SendEmailController : Controller
    {
        // GET: /Administrator/SendEmail/
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Send email";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var listSendEmail =
                        db.SendEmails.Select(a => new
                        {
                            a.ID,
                            a.Title
                        }).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = listSendEmail, TotalRecordCount = listSendEmail.Count });
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
            ViewBag.Title = "Add new";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ESendEmail model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (
                            db.SendEmails.Any(
                                a =>  a.Type == model.Type))
                        {
                            TempData["Messages"] = "Record is exist";
                            return RedirectToAction("Index");
                        }
                        var sendEmail = new SendEmail
                        {
                            Title = model.Title,
                            Type = model.Type,
                            Content = model.Content,
                            ContentDefault = model.Content,
                            Success = model.Success,
                            Error = model.Error
                        };

                        db.SendEmails.InsertOnSubmit(sendEmail);
                        db.SubmitChanges();

                        TempData["Messages"] = "Successful";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        LoadData();
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View();
                    }
                }
                LoadData();
                return View();
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.Title = "Update";
            using (var db = new MyDbDataContext())
            {
                SendEmail sendEmail = db.SendEmails.FirstOrDefault(a => a.ID == id);

                if (sendEmail == null)
                {
                    TempData["Messages"] = "Record is exist ";
                    return RedirectToAction("Index");
                }

                var eSendEmail = new ESendEmail
                {
                    ID = sendEmail.ID,
                    Type = sendEmail.Type,
                    Title = sendEmail.Title,
                    Content = sendEmail.Content,
                    Success = sendEmail.Success,
                    Error = sendEmail.Error
                };
                return View(eSendEmail);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ESendEmail model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        SendEmail sendEmail = db.SendEmails.FirstOrDefault(b => b.ID == model.ID);
                        if (sendEmail != null)
                        {
                            sendEmail.Title = model.Title;
                            sendEmail.Content = model.Content;
                            sendEmail.Success = model.Success;
                            sendEmail.Error = model.Error;

                            db.SubmitChanges();
                            TempData["Messages"] = "Successful";
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

        public void LoadData()
        {
            var listType = new List<SelectListItem>();
            listType.Add(new SelectListItem
            {
                Text = "Select type",
                Value = "",
            });
            listType.AddRange(TypeSendEmail.ListTypeSendEmail().Select(a => new SelectListItem
            {
                Text = a.Text,
                Value = a.Value.ToString(CultureInfo.InvariantCulture)
            }).ToList());
            ViewBag.Types = listType;
        }
    }
}