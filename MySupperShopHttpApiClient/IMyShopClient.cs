using MySupperShop.Models;

namespace MySupperShop.Interfaces
{
    public interface IMyShopClient
    {
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken); 
        Task<List<Product>> GetProducts(CancellationToken cancellationToken); 
        Task DeleteProduct(Product product, CancellationToken cancellationToken);
        Task UpdateProduct(Product newProduct, CancellationToken cancellationToken);
    }
}
