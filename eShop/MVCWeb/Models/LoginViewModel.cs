using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Models
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được bỏ trống Username")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được bỏ trống Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}