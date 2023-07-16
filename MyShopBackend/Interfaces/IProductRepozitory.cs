using MyShopBackend.Models;

namespace MyShopBackend.Interfaces
{
    public interface IProductRepozitory
    {
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task<IResult> GetProductById(Guid id, CancellationToken cancellationToken);
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken);
        Task<IResult> UpdateProduct(Guid id, Product newProduct, CancellationToken cancellationToken);
        Task DeleteProduct(Product product, CancellationToken cancellationToken); 
    }
}
