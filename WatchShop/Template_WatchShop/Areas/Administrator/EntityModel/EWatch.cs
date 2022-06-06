using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Template_WatchShop.Areas.Administrator.EntityModel
{
    public class EWatch
    {
        public int ID { get; set; }
        [DisplayName("Tên phòng")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tên phòng")]
        public string Name { get; set; }

        [DisplayName("Số lượng còn lại ")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng còn lại")]
        [Range(1, int.MaxValue, ErrorMessage = "Số số lượng còn lại phải lớn hơn 0.")]
        public int Quality { get; set; }

        [DisplayName("Giá tiền")]
        [Required(ErrorMessage = "Vui lòng nhập giá phòng")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá phòng phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [DisplayName("Đường kính mặt")]
        [Required(ErrorMessage = "Vui lòng nhập đường kính mặt")]
        [Range(1, double.MaxValue, ErrorMessage = "Đường kính mặt phải lớn hơn 0.")]
        public decimal FaceDiameter { get; set; }

        [DisplayName("Độ chống thấm nước")]
        [Required(ErrorMessage = "Vui lòng nhập độ chống thấm nước")]
        [Range(1, double.MaxValue, ErrorMessage = "Độ chống thấm nước phải lớn hơn 0.")]
        public decimal WaterProof { get; set; }

        [DisplayName("Chất liệu mặt")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập chất liệu mặt")]
        public string FaceMaterial { get; set; }

        [DisplayName("Năng lượng sử dụng")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập năng lượng sử dụng")]
        public string EnergyUse { get; set; }

        [DisplayName("Kích thước")]
        [Range(1, double.MaxValue, ErrorMessage = "Kích thước phải lớn hơn 0.")]
        public decimal? Size { get; set; }

        [DisplayName("Chất lượng dây")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập chất lượng dây")]
        public string StringMaterial { get; set; }

        [DisplayName("Xuất xứ")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập xuất xứ")]
        public string MadeIn { get; set; }

        [DisplayName("Bảo hành")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập bảo hành")]
        public string Insurance { get; set; }

        [DisplayName("Miêu tả")]
        
        public string Description { get; set; }
        public int? Index { get; set; }

        [DisplayName("Ảnh đại diện")]
        [MaxLength(300, ErrorMessage = "Tối đa 300 ký tự")]
        [Required(ErrorMessage = "Vui lòng chọn ảnh đại diện")]
        public string Image { get; set; }

        [DisplayName("Giá khuyến mại")]

        public decimal? PriceDiscount { get; set; }
        [DisplayName("Khuyến mại")]
        [Required(ErrorMessage = "Vui lòng nhập khuyến mại nếu không khuyến mại nhập 0")]
        [Range(0, 100, ErrorMessage = "Khuyến mại phải lớn hơn 0 và nhỏ hơn 100.")]
        public decimal Percent { get; set; }
        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }
        [DisplayName("Sản phẩm hot")]
        public bool Hot { get; set; }
        [DisplayName("Sản phẩm mới")]
        public bool New { get; set; }

        [DisplayName("Alias")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập Alias")]
        public string Alias { get; set; }

        [DisplayName("Hãng sản xuất")]
        [Required(ErrorMessage = "Vui lòng chọn hãng sản xuất")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn hãng sản xuất")]
        public int CatageryID { get; set; }

        [DisplayName("Loại đồng hồ")]
        [Required(ErrorMessage = "Vui lòng chọn loại đồng hồ")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại đồng hồ")]
        public int TypeID { get; set; }


        [DisplayName("Tiêu đề trang")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string MetaTitle { get; set; }

        [DisplayName("Thẻ mô tả")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string MetaDescription { get; set; }
        public List<EGalleryITem> EGalleryITems { get; set; }

    }
    public class EGalleryITem
    {
        public string Image { get; set; }
    }
}