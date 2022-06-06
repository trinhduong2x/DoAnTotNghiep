using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Template_WatchShop.Areas.Administrator.EntityModel
{
    public class ESlide
    {
        public int ID { get; set; }
        [DisplayName("Tên slide")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tên slide")]
        public string Title { get; set; }
        [DisplayName("Hình ảnh")]
        [Required(ErrorMessage = "Vui lòng chọn hình ảnh chạy slide")]
        public string Image { get; set; }
        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }
        public int? Index { get; set; }
    }
}