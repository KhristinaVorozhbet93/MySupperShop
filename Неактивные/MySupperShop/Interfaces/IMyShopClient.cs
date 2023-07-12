using MySupperShop.Models;

namespace MySupperShop.Interfaces
{
    public interface IMyShopClient
    {
        Task AddProduct(Product product);
        Task<Product> GetProduct(Guid id);
    }
}
