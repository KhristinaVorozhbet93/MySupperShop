using MySupperShop.Models;

namespace MySupperShop.Interfaces
{
    public interface ICatalog
    {
        List<Product> GetProducts();
        void AddProduct(Product product);
        Product GetProductById(Guid id);
        void DeleteProduct(Guid productId);
        void UpdateProduct(Guid productId, Product newProduct);
        void ClearCatalog();
    }
}
