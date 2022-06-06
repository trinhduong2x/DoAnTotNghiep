using System.ComponentModel;


namespace Template_WatchShop.Areas.Administrator.ModelShow
{
    public class ShowWatch
    {
        public int ID { get; set; }

        [DisplayName("Tên đồng hồ")]
        public string Name { get; set; }

       
        [DisplayName("Số lượng còn lại")]
        public int Quality { get; set; }
        [DisplayName("Giá bán")]
        public decimal Price { get; set; }
        public int? Index { get; set; }

        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }

     

        [DisplayName("Đồng hồ hot")]
        public bool Hot { get; set; }
    

        [DisplayName("Đồng hồ mới nổi bật")]
        public bool New { get; set; }

        public int CategaryID { get; set; }
        public string Image { get; set; }
    }
}