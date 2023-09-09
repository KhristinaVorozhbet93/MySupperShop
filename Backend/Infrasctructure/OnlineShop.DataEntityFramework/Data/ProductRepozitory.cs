using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Data
{
    public class ProductRepozitory : EfRepozitory<Product>, IProductRepozitory
    {
        public ProductRepozitory(AppDbContext dbContext) : base(dbContext) { }

        public async override Task Update(Product newProduct, CancellationToken cancellationToken)
        {
            var product = await _entities
                .FirstAsync(it => it.Id == newProduct.Id, cancellationToken);

            product!.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.ProducedAt = newProduct.ProducedAt;
            product.ExpiredAt = newProduct.ExpiredAt;
            product.Description = newProduct.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
