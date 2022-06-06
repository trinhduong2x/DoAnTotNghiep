using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectLibrary.Database;

namespace Template_WatchShop.Models
{
    public class CartItem
    {
        public Watch _product { get; set; }
        public int _quantity { get; set; }
        public double Price { set; get; }
        public double totalMoney
        {
            get { return Price * _quantity; }
        }
    }
    public class Cart
    {
        public Watch _sp { get; set; }

        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public void Add(Watch _pro, int _quantity = 1)
        {
            var item = items.FirstOrDefault(s => s._product.ID == _pro.ID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _product = _pro,
                    _quantity = _quantity,
                });

            }
            else
            {
                item._quantity += _quantity;
            }
        }
        public void remove(Watch _pro)
        {
            var item = items.FirstOrDefault(s => s._product.ID == _pro.ID);
            if (item != null)
            {
                items.RemoveAll(n => n._product.ID == _pro.ID);

            }
        }
        public void updateCart(int id, int _quantity)
        {
            var item = items.Find(s => s._product.ID == id);
            if (item != null)
            {
                item._quantity = _quantity;
            }
        }
    }
}