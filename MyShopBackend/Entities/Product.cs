using MyShopBackend.Interfaces;

namespace MyShopBackend.Models
{
    public class Product : IEntity
    {
        public Guid Id { get; init; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ProducedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
