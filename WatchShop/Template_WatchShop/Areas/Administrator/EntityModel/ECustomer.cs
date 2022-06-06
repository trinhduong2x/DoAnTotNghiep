using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Template_WatchShop.Areas.Administrator.EntityModel
{
    public class ECustomer
    {
        public int ID { get; set; }
        [DisplayName("Tên khách hàng")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string Name { get; set; }
        [DisplayName("Hình ảnh")]
        [Required(ErrorMessage = "Vui lòng chọn hình ảnh ")]
        public string Image { get; set; }
        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }
        [DisplayName("Nghề nghiệp")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập nghề nghiệp")]
        public string job { get; set; }
    }
}