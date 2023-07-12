using MySupperShop.Models;

namespace MySupperShop.Interfaces
{
    public interface IMyShopClient
    {
        Task AddProduct(Product product);
        Task<Product> GetProduct(Guid id); 
        Task<List<Product>> GetProducts(); 
        Task DeleteProduct(Product product);
        Task UpdateProduct(Product product);
    }
}
