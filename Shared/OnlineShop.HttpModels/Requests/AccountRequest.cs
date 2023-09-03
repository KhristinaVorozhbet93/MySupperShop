using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModels.Requests
{
    public class AccountRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "Логин минимум 6 символов", MinimumLength = 6)]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
