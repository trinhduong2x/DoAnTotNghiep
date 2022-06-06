using System.ComponentModel;


namespace Template_WatchShop.Areas.Administrator.ModelShow
{
    public class ShowSlider
    {
        public int ID { get; set; }

        [DisplayName("Tên slide ảnh")]
        public string Title { get; set; }

        [DisplayName("Hình ảnh")]
        public string Image { get; set; }

       
     
        public bool Status { get; set; }

    

        [DisplayName("Thứ tự hiển thị")]
        public int? Index { get; set; }
    }
}