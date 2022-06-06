using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template_WatchShop.Models;
using ProjectLibrary.Database;
using ProjectLibrary.Config;

namespace Template_WatchShop.Controllers
{
    public class CartsController : Controller
    {
        MyDbDataContext db = new MyDbDataContext();

        // GET: Carts
        [HttpGet]
        public ActionResult Index()
        {
            Cart lstCarts = getCart();
            if (Session["Cart"] == null)
                return RedirectToAction("Index", "Carts");
            Cart lstCart = Session["Cart"] as Cart;
            ViewBag.totalQuantity = totalQuantity();
            ViewBag.totalMoney = totalMoney();
            return View(lstCart);
        }

        public Cart getCart()
        {
            Cart lstCart = Session["Cart"] as Cart;
            if (lstCart == null || Session["Cart"] == null)
            {
                lstCart = new Cart();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }

        // them gio hang trang sp
        public ActionResult addItem(int id, string strURL)
        {
            Cart lstCart = Session["Cart"] as Cart;
            if (lstCart == null || Session["Cart"] == null)
            {
                lstCart = new Cart();
                Session["Cart"] = lstCart;
            }
           
            var sp = db.Watches.SingleOrDefault(s => s.ID == id);//lấy sp trong bảng
            var pro = lstCart.Items.SingleOrDefault(s => s._product.ID == id);//lấy sp trong giỏ hàng
            if (sp != null && pro == null)
            {
                getCart().Add(sp);
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
            }
            if (sp != null && pro != null)
            {
                getCart().Add(sp);
                Session["count"] = Convert.ToInt32(Session["count"]);
            }

            return Redirect(strURL);
        }



        //add cart cho details
        public ActionResult addToCart(int id, int quantity)
        {

            Cart lstCart = Session["Cart"] as Cart;
            if (lstCart == null || Session["Cart"] == null)
            {
                lstCart = new Cart();
                Session["Cart"] = lstCart;
            }
           
            var sp = db.Watches.SingleOrDefault(s => s.ID == id);

            var pro = lstCart.Items.SingleOrDefault(s => s._product.ID == id);//lấy sp trong giỏ hàng
            if (sp != null && pro == null)
            {
                getCart().Add(sp, quantity);
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
            }
            if (sp != null && pro != null)
            {
                getCart().Add(sp, quantity);
                Session["count"] = Convert.ToInt32(Session["count"]);
            }

            //return RedirectToAction("Index", "Carts");
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });

        }

        private int isExist(int id)
        {
            List<CartItem> lstCart = (List<CartItem>)Session["Cart"];
            for (int i = 0; i < lstCart.Count; i++)
                if (lstCart[i]._product.ID.Equals(id))
                    return i;
            return -1;
        }
        //lấy danh sách giỏ hàngg
        public ActionResult layDS()
        {
            return View((List<CartItem>)Session["Cart"]);
        }
        private int totalQuantity()
        {
            int iQuantity = 0;
            Cart lstcart = Session["Cart"] as Cart;
            if (lstcart != null)
            {
                iQuantity = lstcart.Items.Sum(s => s._quantity);
            }
            return iQuantity;
        }
        private double totalMoney()
        {
            double itotalMoney = 0;
            Cart lstCarts = Session["Cart"] as Cart;
            if (lstCarts != null)
            {
                itotalMoney = lstCarts.Items.Sum(n => n._product.Quality * Convert.ToDouble(n._product.Price ));
            }

            return itotalMoney;
        }

        //cập nhật giỏ hảng
        public ActionResult update(int ID, int Quantity)
        {
            Cart lstCart = Session["Cart"] as Cart;
           
            var pro = CommentController.detailWatch(ID);
            int soluong = pro.watch.Quality - Quantity;
            if(Quantity < 0)
            {
                TempData["Messages"] = "Vui long nhap so duong!";
                return RedirectToAction("Index", "Carts");
            }
            else
            {
                if (soluong < 0)
                {
                    TempData["Messages"] = "So luong da vuot qua!";
                }
                else
                {
                    TempData["Messages"] = "Cap nhat thanh cong!";
                    lstCart.updateCart(ID, Quantity);
                }
                return RedirectToAction("Index", "Carts");
            }
           
           
        }
        //Xóa tất cả trong giỏ hàng
        public ActionResult XoaTatCa()
        {
            Session["Cart"] = null;
            Session["count"] = 0;
            //return RedirectToAction("Index", "Carts");
            return Json(new { status = true, JsonRequestBehavior.AllowGet });

        }

        //xoa sp
        public ActionResult Xoa(int id)
        {
            Cart lstCart = Session["Cart"] as Cart;
            var sp = lstCart.Items.SingleOrDefault(s => s._product.ID == id);
            if (sp != null)
            {
                lstCart.remove(sp._product);
                Session["count"] = Convert.ToInt32(Session["count"]) - 1;
                Session["Cart"] = lstCart;
            }
            return RedirectToAction("Index", "Carts");
        }



        [HttpPost]
        public ActionResult SendBooking(Order model)
        {
            string status = "success";
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        Watchshop watchshop = CommentController.DetailShop();
                        string codeBooking = watchshop.CodeBooking + "1";
                        if (db.Orders.Any())
                        {
                            codeBooking = watchshop.CodeBooking +
                                          (db.Orders.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1);
                        }
                        model.Code = codeBooking;
                        string infoBooking = "";


                        Cart lstCart = Session["Cart"] as Cart;
                        
                        foreach (CartItem item in lstCart.Items)
                        {

                            if (item._quantity > 0)
                            {
                                infoBooking += item._product.Name + " = " + item._quantity + ", ";


                            }
                        }

                        model.DateOrder = DateTime.Now;
                        model.InfoBooking = infoBooking;
                        model.Completed = 0;
                        db.Orders.InsertOnSubmit(model);
                        db.SubmitChanges();
                        int orderID = model.ID;
                        foreach (CartItem item in lstCart.Items)
                        {
                            Order_Watch order_Watch = new Order_Watch();

                            if (item._quantity > 0)
                            {
                                infoBooking += item._product.Name + " = " + item._quantity + ", ";
                                order_Watch.OrderID = orderID;
                                order_Watch.ProducID = item._product.ID;
                                order_Watch.Quality = item._quantity;
                                db.Order_Watches.InsertOnSubmit(order_Watch);
                                db.SubmitChanges();
                                Watch watch = db.Watches.Where(a => a.ID == item._product.ID).FirstOrDefault();
                                watch.Quality = watch.Quality - item._quantity;
                                db.SubmitChanges();

                            }
                        }
                        //Gửi email xác nhận đặt phòng
                        SendEmail sendEmail =
                        db.SendEmails.FirstOrDefault(
                            a => a.Type == TypeSendEmail.Order);

                        sendEmail.Title = sendEmail.Title.Replace("{ShopName}", watchshop.Name);
                        string content = sendEmail.Content;
                        content = content.Replace("{Code}", model.Code);
                        content = content.Replace("{Gender}", model.Gender);
                        content = content.Replace("{FullName}", model.FullName);
                        content = content.Replace("{Email}", model.Email);
                        content = content.Replace("{Tel}", model.Phone);
                        content = content.Replace("{Address}", model.Address);
                        content = content.Replace("{Request}", model.Request);
                        content = content.Replace("{InfoBooking}", model.InfoBooking);                     
                        content = content.Replace("{TotalPrice}", model.TotalMoney.ToString("N"));
                        content = content.Replace("{ShopName}", watchshop.Name);
                        content = content.Replace("{ShopEmail}", watchshop.Email);
                        content = content.Replace("{ShopTel}", watchshop.Tel);
                        content = content.Replace("{Website}", watchshop.Website);

                        MailHelper.SendMail(model.Email, sendEmail.Title, content);
                        MailHelper.SendMail(watchshop.Email, watchshop.Name + " (" + model.Code + ")- Đặt hàng của " + model.FullName, content);
                        Session.Clear();
                    }
                }
                catch (Exception ex)
                {
                    status = "error";
                }
            }
            return Redirect("/Carts/Messages/?status=" + status);
        }

        [HttpGet]
        public ActionResult Messages()
        {
            using (var db = new MyDbDataContext())
            {
                SendEmail sendEmail =
                       db.SendEmails.FirstOrDefault(
                           a => a.Type == TypeSendEmail.Order );

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