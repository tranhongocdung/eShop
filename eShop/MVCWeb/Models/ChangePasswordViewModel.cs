using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Models
{
    public class ChangePasswordViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được bỏ trống Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được bỏ trống Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}