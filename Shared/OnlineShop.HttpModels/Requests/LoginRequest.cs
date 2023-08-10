using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModels.Requests
{
    public class LoginRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "Логин минимум 6 символов", MinimumLength = 6)]
        public string Login { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль минимум 8 символов.", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
