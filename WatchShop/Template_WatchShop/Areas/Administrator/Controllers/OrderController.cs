using ProjectLibrary.Config;
using ProjectLibrary.Database;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Areas.Administrator.Models;

namespace Template_WatchShop.Areas.Administrator.Controllers
{
    public class OrderController : Controller
    {
        
        // GET: Administrator/Order
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang đặt hàng";
            ListData();
            ViewBag.BookingOnline = true;
            return View();
        }

        public ActionResult OrderList()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang đơn hàng đang giao hàng";
            ViewBag.BookingOnline = true;
            return View();
        }
        public ActionResult OrderListfalse()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang đặt hàng bị hủy";
            ViewBag.BookingOnline = true;
            return View();
        }
        public ActionResult OrderListRecived()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang đặt hàng đã nhận đơn";
            ViewBag.BookingOnline = true;
            return View();
        }
        public ActionResult OrderListShipping()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang đơn hàng đã hoàn thành ";
            ViewBag.BookingOnline = true;
            return View();
        }
        [HttpPost]
        public JsonResult List(string checkin = "", string checkout = "",string bookdate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    decimal total = 0;
                    List<Order> orders = db.Orders.OrderByDescending(a => a.ID).ToList();

                    CultureInfo provider = new CultureInfo("en-US");

                    if (string.IsNullOrEmpty(checkin) == false)
                    {
                        DateTime book_to = DateTime.Parse(checkin, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date >= book_to.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(checkout) == false)
                    {
                        DateTime book_from = DateTime.Parse(checkout, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date <= book_from.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(bookdate) == false)
                    {
                        DateTime book_date = DateTime.Parse(bookdate, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date == book_date.Date).OrderBy(a => a.DateOrder).ToList();
                    }
                    var records = orders.Select(a => new
                    {
                        a.ID,
                        a.Code,                                          
                        a.InfoBooking,
                        a.Gender,
                        FullName = a.Gender + ". " + a.FullName,
                        DateBook = a.DateOrder.ToString("MMMM, dd, yyyy"),
                        a.Completed,
                        a.TotalMoney,
                        
                        
                    }).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    foreach (var item in orders)
                    {
                        total = item.TotalMoney + total;
                    }
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = orders.Count(),total = total });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ListOrder(string checkin = "", string checkout = "", string bookdate = "")
        {
            Session["products"] = null;
            Session["Checkin"] = null;
            Session["Checkout"] = null;
            Session["Total"] = null;
            try
            {
                using (var db = new MyDbDataContext())
                {
                    
                    List<Order> orders = db.Orders.Where(a => a.Completed == 1).OrderByDescending(a => a.ID).ToList();

                    CultureInfo provider = new CultureInfo("en-US");

                    if (string.IsNullOrEmpty(checkin) == false)
                    {
                        DateTime book_to = DateTime.Parse(checkin, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date >= book_to.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(checkout) == false)
                    {
                        DateTime book_from = DateTime.Parse(checkout, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date <= book_from.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(bookdate) == false)
                    {
                        DateTime book_date = DateTime.Parse(bookdate, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date == book_date.Date).OrderBy(a => a.DateOrder).ToList();
                    }
                    var records = orders.Select(a => new
                    {
                        a.ID,
                        a.Code,
                        a.InfoBooking,
                        a.Gender,
                        FullName = a.Gender + ". " + a.FullName,
                        DateBook = a.DateOrder.ToString("MMMM, dd, yyyy"),
                        a.Completed,
                        a.TotalMoney,

                    }).ToList();
                    decimal total = 0;
                    foreach (var item in orders)
                    {
                        total = item.TotalMoney + total;
                    }
                    Session["Total"] = total;
                    Session["products"] = orders;
                    Session["Checkin"] = checkin;
                    Session["Checkout"] = checkout;
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = orders.Count(),total = total },JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ListOrderShipping(string checkin = "", string checkout = "", string bookdate = "")
        {
            Session["products"] = null;
            Session["Checkin"] = null;
            Session["Checkout"] = null;
            Session["Total"] = null;
            try
            {
                using (var db = new MyDbDataContext())
                {

                    List<Order> orders = db.Orders.Where(a => a.Completed == 2).OrderByDescending(a => a.ID).ToList();

                    CultureInfo provider = new CultureInfo("en-US");

                    if (string.IsNullOrEmpty(checkin) == false)
                    {
                        DateTime book_to = DateTime.Parse(checkin, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date >= book_to.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(checkout) == false)
                    {
                        DateTime book_from = DateTime.Parse(checkout, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date <= book_from.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(bookdate) == false)
                    {
                        DateTime book_date = DateTime.Parse(bookdate, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date == book_date.Date).OrderBy(a => a.DateOrder).ToList();
                    }
                    var records = orders.Select(a => new
                    {
                        a.ID,
                        a.Code,
                        a.InfoBooking,
                        a.Gender,
                        FullName = a.Gender + ". " + a.FullName,
                        DateBook = a.DateOrder.ToString("MMMM, dd, yyyy"),
                        a.Completed,
                        a.TotalMoney,

                    }).ToList();
                    decimal total = 0;
                    foreach (var item in orders)
                    {
                        total = item.TotalMoney + total;
                    }
                    Session["Total"] = total;
                    Session["products"] = orders;
                    Session["Checkin"] = checkin;
                    Session["Checkout"] = checkout;
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = orders.Count(), total = total }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ListOrderfalse(string checkin = "", string checkout = "", string bookdate = "")
        {
            Session["products"] = null;
            Session["Checkin"] = null;
            Session["Checkout"] = null;
            Session["Total"] = null;
            try
            {
                using (var db = new MyDbDataContext())
                {

                    List<Order> orders = db.Orders.Where(a => a.Completed == 3).OrderByDescending(a => a.ID).ToList();

                    CultureInfo provider = new CultureInfo("en-US");

                    if (string.IsNullOrEmpty(checkin) == false)
                    {
                        DateTime book_to = DateTime.Parse(checkin, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date >= book_to.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(checkout) == false)
                    {
                        DateTime book_from = DateTime.Parse(checkout, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date <= book_from.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(bookdate) == false)
                    {
                        DateTime book_date = DateTime.Parse(bookdate, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date == book_date.Date).OrderBy(a => a.DateOrder).ToList();
                    }
                    var records = orders.Select(a => new
                    {
                        a.ID,
                        a.Code,
                        a.InfoBooking,
                        a.Gender,
                        FullName = a.Gender + ". " + a.FullName,
                        DateBook = a.DateOrder.ToString("MMMM, dd, yyyy"),
                        a.Completed,
                        a.TotalMoney,

                    }).ToList();
                    decimal total = 0;
                    foreach (var item in orders)
                    {
                        total = item.TotalMoney + total;
                    }
                    Session["Total"] = total;
                    Session["products"] = orders;
                    Session["Checkin"] = checkin;
                    Session["Checkout"] = checkout;
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = orders.Count(), total }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ListOrderRecived(string checkin = "", string checkout = "", string bookdate = "")
        {
            Session["products"] = null;
            Session["Checkin"] = null;
            Session["Checkout"] = null;
            Session["Total"] = null;
            try
            {
                using (var db = new MyDbDataContext())
                {

                    List<Order> orders = db.Orders.Where(a => a.Completed == 0).OrderByDescending(a => a.ID).ToList();

                    CultureInfo provider = new CultureInfo("en-US");

                    if (string.IsNullOrEmpty(checkin) == false)
                    {
                        DateTime book_to = DateTime.Parse(checkin, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date >= book_to.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(checkout) == false)
                    {
                        DateTime book_from = DateTime.Parse(checkout, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date <= book_from.Date).OrderBy(a => a.DateOrder).ToList();
                    }

                    if (string.IsNullOrEmpty(bookdate) == false)
                    {
                        DateTime book_date = DateTime.Parse(bookdate, provider,
                   DateTimeStyles.AdjustToUniversal);
                        orders =
                            orders.Where(a => a.DateOrder.Date == book_date.Date).OrderBy(a => a.DateOrder).ToList();
                    }
                    var records = orders.Select(a => new
                    {
                        a.ID,
                        a.Code,
                        a.InfoBooking,
                        a.Gender,
                        FullName = a.Gender + ". " + a.FullName,
                        DateBook = a.DateOrder.ToString("MMMM, dd, yyyy"),
                        a.Completed,
                        a.TotalMoney,

                    }).ToList();
                    decimal total = 0;
                    foreach (var item in orders)
                    {
                        total = item.TotalMoney + total;
                    }
                    Session["Total"] = total;
                    Session["products"] = orders;
                    Session["Checkin"] = checkin;
                    Session["Checkout"] = checkout;
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = orders.Count(), total }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        [HttpGet]
        public ActionResult Detail(int Id)
        {
            using (var db = new MyDbDataContext())
            {
                Order detailOrder = db.Orders.FirstOrDefault(a => a.ID == Id);
                Session["detailOrder"] = detailOrder;

                if (detailOrder == null)
                {
                    return RedirectToAction("Index");
                }
                return View("Detail", detailOrder);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.Title = "Cập nhật đơn hàng";
            using (var db = new MyDbDataContext())
            {
                Order order = db.Orders.FirstOrDefault(a => a.ID == id);

                if (order == null)
                {
                    TempData["Messages"] = "Đơn hàng không tồn tại";
                    return RedirectToAction("Index");
                }
                ListData();
                return View(order);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Order model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                   
                    try
                    {
                        Order order = db.Orders.FirstOrDefault(b => b.ID == model.ID);
                       
                        if (order != null)
                        {
                            if(order.Completed == 0)
                            {
                                if(model.Completed == 4)
                                {
                                   
                                    List<Order_Watch> order_Watches = db.Order_Watches.Where(a => a.OrderID == model.ID).ToList();
                                    foreach (var item in order_Watches)
                                    {
                                        Watch watch = db.Watches.Where(a => a.ID == item.ProducID).FirstOrDefault();
                                        watch.Quality += item.Quality;
                                        db.SubmitChanges();
                                    }
                                    order.Completed = model.Completed;
                                    db.SubmitChanges();
                                    TempData["Messages"] = "Cập nhật đơn hàng thành công";
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    order.Completed = model.Completed;

                                    db.SubmitChanges();

                                    TempData["Messages"] = "Cập nhật đơn hàng thành công";
                                    return RedirectToAction("Index");
                                }
                            }
                            if(order.Completed == 1)
                            {
                                if(model.Completed == 0 || model.Completed == 4)
                                {
                                    TempData["Messages"] = "Cập nhật đơn hàng thành công";
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    order.Completed = model.Completed;

                                    db.SubmitChanges();

                                    TempData["Messages"] = "Cập nhật đơn hàng thành công";
                                    return RedirectToAction("Index");
                                }
                            }
                            if(order.Completed == 2)
                            {
                                if(model.Completed == 2)
                                {
                                    order.Completed = model.Completed;

                                    db.SubmitChanges();

                                    TempData["Messages"] = "Cập nhật đơn hàng thành công";
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Messages"] = "Đơn hàng KHÔNG THỂ đổi";
                                    return RedirectToAction("Index");
                                }
                            }
                            if (order.Completed == 3)
                            {
                                if (model.Completed == 3)
                                {
                                    order.Completed = model.Completed;

                                    db.SubmitChanges();

                                    TempData["Messages"] = "Cập nhật đơn hàng thành công";
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Messages"] = "Đơn hàng KHÔNG THỂ  đổi";
                                    return RedirectToAction("Index");
                                }
                            }
                            if (order.Completed == 4)
                            {
                                if (model.Completed == 4)
                                {
                                    order.Completed = model.Completed;

                                    db.SubmitChanges();

                                    TempData["Messages"] = "Cập nhật đơn hàng thành công";
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Messages"] = "Đơn hàng KHÔNG THỂ đổi";
                                    return RedirectToAction("Index");
                                }
                            }

                        }
                    }
                    catch (Exception exception)
                    {
                        ListData();
                        ViewBag.Messages = "Lỗi: " + exception.Message;
                        return View();
                    }
                }
                ListData();
                return View(model);
            }
        }


        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var db = new MyDbDataContext();
            Order del = db.Orders.FirstOrDefault(a => a.ID == Id);
            List<Order_Watch> order_Watch = db.Order_Watches.Where(a => a.OrderID == Id).ToList();

            if (del == null)
            {
                return Json(new { Result = "ERROR", Message = "Đơn hàng không tồn tại." });
            }
            foreach (var item in order_Watch)
            {
                db.Order_Watches.DeleteOnSubmit(item);
            }
          

            db.Orders.DeleteOnSubmit(del);
            db.SubmitChanges();
            return Json(new { Result = "OK", Message = "Xóa thành công." });
        }

        public ActionResult ExportPDF()
        {
            return new ViewAsPdf("ExportPDF");
        }

        public ActionResult ExportPDF1()
        {
            return new ViewAsPdf("ExportPDF1");
        }

        public ActionResult ExportPDF2()
        {
            return new ViewAsPdf("ExportPDF2");
        }

        public ActionResult ExportPDF3()
        {
            return new ViewAsPdf("ExportPDF3");
        }
        public ActionResult ExportPDF4()
        {
            return new ViewAsPdf("ExportPDF4");

        }
        public void ListData()
        {
            var listTypeOrder = new List<SelectListItem>();
            listTypeOrder.Add(new SelectListItem
            {
                Text = "Chọn kiểu đơn hàng",
                Value = "0",
            });
            listTypeOrder.AddRange(SystemOrderType.OrderType.Select(a => new SelectListItem
            {
                Text = a.Value,
                Value = a.Key.ToString(CultureInfo.InvariantCulture)
            }).ToList());
            ViewBag.ListTypeOrder = listTypeOrder;
        }
        ////thống kê sản phẩm bán
        //[HttpPost]
        //[ValidateInput(false)]
        public ActionResult ListProduct(string checkin, string checkout , string bookdate)
        {
            using (var db = new MyDbDataContext())
            {

                   
                    List<Product_Order> product = db.Watches.Where(a => a.Status)
                    .Join(db.Order_Watches, w => w.ID, ow => ow.ProducID, (w, ow) => new { w, ow })
                    .Join(db.Orders.Where(a => a.Completed == SystemOrderType.Completed), oow => oow.ow.OrderID, o => o.ID, (oow, o) => new { oow, o }).Select(m => new Product_Order
                    {
                        ID = m.oow.ow.ProducID,
                        Name = m.oow.w.Name,
                        Price = m.oow.w.Price,
                        PriceDiscount = m.oow.w.PriceDiscount,
                        Quality = m.oow.ow.Quality,
                        CreateDate = m.o.DateOrder,
                        Total = m.o.TotalMoney,

                    }).OrderBy(a => a.CreateDate).ToList();

                CultureInfo provider = new CultureInfo("en-US");

                if (string.IsNullOrEmpty(checkin) == false)
                {
                    DateTime book_to = DateTime.Parse(checkin, provider,
               DateTimeStyles.AdjustToUniversal);
                    product =
                        product.Where(a => a.CreateDate.Date >= book_to.Date).OrderBy(a => a.CreateDate).ToList();
                }

                if (string.IsNullOrEmpty(checkout) == false)
                {
                    DateTime book_from = DateTime.Parse(checkout, provider,
               DateTimeStyles.AdjustToUniversal);
                    product =
                        product.Where(a => a.CreateDate.Date <= book_from.Date).OrderBy(a => a.CreateDate).ToList();
                }

                if (string.IsNullOrEmpty(bookdate) == false)
                {
                    DateTime book_date = DateTime.Parse(bookdate, provider,
               DateTimeStyles.AdjustToUniversal);
                    product =
                        product.Where(a => a.CreateDate.Date == book_date.Date).OrderBy(a => a.CreateDate).ToList();
                }

                var summary = from p in product
                              let k = new
                              {
                                  //try this if you need a date field 
                                  //   p.SaleDate.Date.AddDays(-1 *p.SaleDate.Day - 1)
                                  ID = p.ID,
                                  Name = p.Name,
                              }
                              group p by k into t
                              select new
                              {
                                  ID = t.Key.ID,
                                  Name = t.Key.Name,
                                  Qty = t.Sum(p => p.Quality)
                              };

                var item = summary.Where(a => a.Qty == summary.Max(b => b.Qty)).FirstOrDefault();
                Session["name"] = 0;
                Session["quanlity"] = 0;
                Session["products"] = null;

                if (item != null)
                {
                    var name = item.Name;
                    var quanlity = item.Qty;
                    ViewBag.name = name;
                    ViewBag.quanlity = quanlity;
                    ViewBag.Checkin = checkin;
                    ViewBag.Product = product;
                    ViewBag.Checkout = checkout;

                    Session["products"] = product;
                    Session["name"] = name;
                    Session["quanlity"] = quanlity;
                  
                }
                Session["Checkin"] = checkin;
                Session["Checkout"] = checkout;
                return View(product);
            }
        }
        public ActionResult ListProductPDF()
        {          
            return new ViewAsPdf("ListProductPDF");
        }
       
    }
}