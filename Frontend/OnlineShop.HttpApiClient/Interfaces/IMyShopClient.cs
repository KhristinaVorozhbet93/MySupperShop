using OnlineShop.HttpApiClient.Models;
using OnlineShop.HttpModels.Requests;

namespace OnlineShop.HttpApiClient.Interfaces
{
    public interface IMyShopClient
    {
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<List<Product>> GetProducts(CancellationToken cancellationToken);
        Task DeleteProduct(Product product, CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
        Task Register(RegisterRequest account, CancellationToken cancellationToken);
    }
}
