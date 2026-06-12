using System.ComponentModel.DataAnnotations;

namespace Library.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ReturnUrl { get; set; } = "/";

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

}
