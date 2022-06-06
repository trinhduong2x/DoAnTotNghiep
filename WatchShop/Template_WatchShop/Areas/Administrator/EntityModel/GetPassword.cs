
using System.ComponentModel.DataAnnotations;

namespace Template_WatchShop.Areas.Administrator.EntityModel
{
    public class GetPassword
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "vui lòng nhập đúng đại chỉ email đã đăng ký")]
        [EmailAddress(ErrorMessage = "vui lòng nhập đúng đại chỉ email đã đăng ký")]
        public string Email { get; set; }
    }
}