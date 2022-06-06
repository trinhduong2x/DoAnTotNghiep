using System;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;

namespace Template_WatchShop.Controllers
{
    public class SendContactController : Controller
    {
        [HttpPost]
        public ActionResult SubmitContact(Contact model)
        {
            model.CreateDate = DateTime.Now;
            
                using (var db = new MyDbDataContext())
                {
                    model.CreateDate = DateTime.Now;
                    db.Contacts.InsertOnSubmit(model);
                    db.SubmitChanges();

                    SendEmail sendEmail =
                        db.SendEmails.FirstOrDefault(
                            a => a.Type == TypeSendEmail.Contact);
                    Watchshop watchshop = CommentController.DetailShop();

                    sendEmail.Title = sendEmail.Title.Replace("{NameShop}", watchshop.Name);
                    string content = sendEmail.Content;
                    content = content.Replace("{FullName}", model.FullName);
                    content = content.Replace("{Tel}", model.Tel);
                    content = content.Replace("{Email}", model.Email);
                    content = content.Replace("{Request}", model.Request);

                    content = content.Replace("{NameHotel}", watchshop.Name);
                    content = content.Replace("{TelHotel}", watchshop.Tel);
                    content = content.Replace("{EmailHotel}", watchshop.Email);
                    content = content.Replace("{AddressHotel}", watchshop.Address);
                    content = content.Replace("{Website}", watchshop.Website);

                    MailHelper.SendMail(model.Email, sendEmail.Title, content);
                    MailHelper.SendMail(watchshop.Email, watchshop.Name + " Contact of " + model.FullName, content);
                    return Redirect("/Contact/Messages?status=success");
                }
            
        }

        [HttpGet]
        public ActionResult Messages()
        {
            using (var db = new MyDbDataContext())
            {
                SendEmail sendEmail =
                       db.SendEmails.FirstOrDefault(
                           a => a.Type == TypeSendEmail.Contact);

                string status = Request.Params["status"];
                ViewBag.Messages = "";
                if (string.IsNullOrEmpty(status) == false)
                {
                    if (status.Equals("success"))
                    {
                        ViewBag.Messages = sendEmail.Success;
                    }
                    else
                    {
                        ViewBag.Messages = sendEmail.Error;
                    }
                }
                else
                {
                    ViewBag.Messages = sendEmail.Error;
                }
                return View();
            }
        }
    }
}