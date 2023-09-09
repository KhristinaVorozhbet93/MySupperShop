using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class CatalogService
    {
        private readonly IUnitOfWork _uow;

        public CatalogService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException(nameof(uow));
        }
        public virtual async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var product = await _uow.ProductRepozitory.GetById(id, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(nameof(product));
            }
            return product;
        }

        public virtual async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _uow.ProductRepozitory.GetAll(cancellationToken);

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
            await _uow.ProductRepozitory.Add(product, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateProduct(Guid productId,
            string name, string description, decimal price, 
            DateTime producedAt, DateTime expiredAt, string image,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(name));
            ArgumentNullException.ThrowIfNull(nameof(description));
            ArgumentNullException.ThrowIfNull(nameof(image));
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));

            var product = await _uow.ProductRepozitory.GetById(productId, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException("Продукт с таким id не найден!");
            }
            product.Name = name; 
            product.Description = description;
            product.Price = price;
            product.ProducedAt = producedAt;
            product.ExpiredAt = expiredAt;
            product.Image = image;

            await _uow.ProductRepozitory.Update(product, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task DeleteProduct(Guid id,CancellationToken cancellationToken)
        {
            var product = await _uow.ProductRepozitory.GetById(id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(nameof(product));
            }
            await _uow.ProductRepozitory.Delete(product, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
