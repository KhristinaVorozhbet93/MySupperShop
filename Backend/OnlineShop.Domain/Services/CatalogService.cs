using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class CatalogService
    {
        private readonly IRepozitory<Product> _repozitory;

        public CatalogService(IRepozitory<Product> repozitory)
        {
            _repozitory = repozitory ?? throw new ArgumentException(nameof(repozitory));
        }
        public virtual async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var product = await _repozitory.GetById(id, cancellationToken);

            if (product is null)
            {
                throw new InvalidOperationException(nameof(product));
            }
            return product;
        }

        public virtual async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _repozitory.GetAll(cancellationToken);

            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }
            return products;
        }

        public virtual async Task AddProduct(string name, string description,
                    decimal price,DateTime producedAt,DateTime expiredAt, string image, 
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(image);

            var product = new Product(Guid.Empty, name,description, price, 
                producedAt, expiredAt, image); 
            await _repozitory.Add(product, cancellationToken);
        }

        public virtual async Task DeleteProduct(Guid id,CancellationToken cancellationToken)
        {
            var product = await _repozitory.GetById(id, cancellationToken);
            if (product is null)
            {
                throw new InvalidOperationException();
            }
            await _repozitory.Delete(product, cancellationToken);
        }
    }
}
