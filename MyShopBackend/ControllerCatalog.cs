using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend
{
    public class ControllerCatalog : ControllerBase
    {
        private readonly IRepozitory<Product> _repozitory;
        private readonly IProductRepozitory _productRepozitory;

        public ControllerCatalog(IRepozitory<Product> repozitory, IProductRepozitory productRepozitory)
        {
            _repozitory = productRepozitory;
            _productRepozitory = productRepozitory;

        }
        [HttpPost ("add_product")]
        public async Task AddProduct(Product product,CancellationToken cancellationToken)
        {
            await _repozitory.Add(product, cancellationToken);
        }
        [HttpGet("get_product")]
        public async Task<Product> GetProductById(Guid id,CancellationToken cancellationToken)
        {
            return await _repozitory.GetById(id, cancellationToken);
        }
        [HttpGet("get_products")]
        public async Task<List<Product>> GetAllProducts(CancellationToken cancellationToken)
        {
            return await _repozitory.GetAll(cancellationToken);
        }
        [HttpPost("update_product")]
        public async Task UpdateProduct(Product newProduct,
            
            CancellationToken cancellationToken)
        {
            await _productRepozitory.Update(newProduct, cancellationToken);
        }
        [HttpPost("delete_product")]
        public async Task DeleteProduct(Product product, CancellationToken cancellationToken)
        {
            await _repozitory.Delete(product, cancellationToken);
        }
    }
}
