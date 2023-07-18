using MyShopBackend.Interfaces;

namespace MyShopBackend.Models
{
    public class Account : IEntity
    {
        public Guid Id { get; init; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
