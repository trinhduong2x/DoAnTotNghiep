using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.EntityModel;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Administrator/Customer
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang khách hàng";
            return View();
        }

      

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Customer> customers = db.Customers.ToList();
                    List<Customer> records = customers.OrderBy(c => c.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = customers.Count });
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
            var model = new ECustomer();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ECustomer model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        var customer = new Customer
                        {

                            Name = model.Name,
                            job = model.job,
                            Image = model.Image,
                            Status = model.Status,
                        };

                        db.Customers.InsertOnSubmit(customer);
                        db.SubmitChanges();

                        TempData["Messages"] = "Thêm mới khách hàng thành công";
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
                Customer customer = db.Customers.FirstOrDefault(a => a.ID == id);
                if (customer == null)
                {
                    TempData["Messages"] = "khách hàng không tồn tại";
                    return RedirectToAction("Index");
                }
                var eCustomer = new ECustomer
                {
                    ID = customer.ID,
                    Name = customer.Name,
                    job = customer.job,
                    Image = customer.Image,
                    Status = customer.Status,
                };
                ViewBag.Title = "Sửa slide";
                return View(eCustomer);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ECustomer model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        Customer customer = db.Customers.FirstOrDefault(b => b.ID == model.ID);

                        if (customer != null)
                        {
                            customer.Name = model.Name;
                            customer.job = model.job;
                            customer.Image = model.Image;
                            customer.Status = model.Status;
                            db.SubmitChanges();
                            TempData["Messages"] = "Sửa khách hàng thành công.";
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
                    Customer del = db.Customers.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Customers.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa khách hàng thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "khách hàng không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }
}