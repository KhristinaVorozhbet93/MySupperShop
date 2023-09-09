using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModels.Requests
{
    public class CartRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public ProductRequest Product{ get; set; }

        [Required]
        public double Quantity{ get; set; }
    }
}
