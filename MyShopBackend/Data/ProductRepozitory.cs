using Microsoft.EntityFrameworkCore;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend.Data
{
    public class ProductRepozitory : EfRepozitory<Product>, IProductRepozitory
    {
        private readonly AppDbContext _dbContext;

        public ProductRepozitory(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }

        public async override Task Update(Product newProduct, CancellationToken cancellationToken)
        {
            var product = await _entities
                .FirstOrDefaultAsync(it => it.Id == newProduct.Id, cancellationToken);

            product!.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.ProducedAt = newProduct.ProducedAt;
            product.ExpiredAt = newProduct.ExpiredAt;
            product.Description = newProduct.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
