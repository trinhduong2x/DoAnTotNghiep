using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Template_WatchShop.Areas.Administrator.EntityModel
{
    public class EShop
    {
        public int ID { get; set; }

        [DisplayName("Tên cửa hàng")]
        [Required(ErrorMessage = "Vui lòng nhập tên cửa hàng")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string Name { get; set; }

        [DisplayName("Địa chỉ cửa hàng")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ cửa hàng")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Address { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string Tel { get; set; }

        [DisplayName("Địa chỉ email")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "vui lòng nhập đúng địa chỉ email")]
        public string Email { get; set; }

        [DisplayName("Logo khách sạn")]
        [Required(ErrorMessage = "Vui lòng chọn logo")]
        public string Logo { get; set; }

        [DisplayName("Ảnh đại diện")]
        [Required(ErrorMessage = "Vui lòng chọn ảnh đại diện")]
        public string Image { get; set; }
                   

        [DisplayName("Vị trí khách sạn trên google")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Location { get; set; }
        [DisplayName("Ký hiệu mã booking")]
        [Required(ErrorMessage = "Vui lòng nhập ký hiệu mã booking")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 ký tự")]
        public string CodeBooking { get; set; }
        public string Description { get; set; }

        [DisplayName("Link trang facebook")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string FaceBook { get; set; }

        [DisplayName("Link Zalo")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Zalo { get; set; }

        [DisplayName("Copyright")]
        [Required(ErrorMessage = "Vui lòng nhập copyright")]
        [MaxLength(300, ErrorMessage = "Tối đa 300 ký tự")]
        public string CopyRight { get; set; }

        [DisplayName("Tiêu đề trang")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string MetaTitle { get; set; }

        [DisplayName("Thẻ mô tả")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string MetaDescription { get; set; }
     

    }
}