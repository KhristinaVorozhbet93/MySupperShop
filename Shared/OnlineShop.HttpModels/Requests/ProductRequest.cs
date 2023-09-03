using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModels.Requests
{
    public class ProductRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime ProducedAt { get; set; }
        [Required]
        public DateTime ExpiredAt { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
