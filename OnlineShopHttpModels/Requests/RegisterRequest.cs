using System.ComponentModel.DataAnnotations;

namespace MyShopBackend.Models
{
    public class RegisterRequest 
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
