using System.ComponentModel.DataAnnotations;

namespace InterviewsControl.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [Display(Name = "管理員帳號")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; } = string.Empty;
    }
}