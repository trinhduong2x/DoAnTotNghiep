using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template_WatchShop.Areas.Administrator.EntityModel
{
    public class ECategary
    {
        public int ID { get; set; }

        [DisplayName("Tiêu đề quảng cáo")]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề quảng cáo")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string Title { get; set; }

        [DisplayName("Alias")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string Alias { get; set; }
        [DisplayName("Ảnh đại diện")]
        
        public string Image { get; set; }
        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }
        [NotMapped]
        public List<EWatch> ewatch { get; set; }

    }
}