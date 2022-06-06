using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Template_WatchShop.Areas.Administrator.Models
{
    public class Product_Order
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quality { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }     
        public DateTime CreateDate { get; set; }
        public decimal? PriceDiscount { get; set; }
        public decimal Percent { get; set; }


       

    }
}