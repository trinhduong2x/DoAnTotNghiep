using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template_WatchShop.Areas.Administrator.EntityModel
{
    public class ETypeWatch
    {
        public int ID { get; set; }
        [DisplayName("Tiêu đề")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề menu")]
        public string Title { get; set; }
        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }
        [NotMapped]
        public List<EWatch> ewatch { get; set; }


    }
}