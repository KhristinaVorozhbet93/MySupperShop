using Microsoft.EntityFrameworkCore;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend.Data
{
    public class ProductRepozitory : IProductRepozitory
    {
        private readonly AppDbContext _dbContext;
        public ProductRepozitory(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }      
        public async Task AddProduct(Product product,CancellationToken cancellationToken)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<IResult> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
            if (product is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        }
        public async Task<List<Product>> GetAllProducts(CancellationToken cancellationToken)
        {
            return await _dbContext.Products.ToListAsync(cancellationToken);
        }
        public async Task<IResult> UpdateProduct(Product newProduct, 
            CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(product => product.Id == newProduct.Id, cancellationToken);
            if (product is null)
            {
                return Results.NotFound();
            }
            product!.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.ProducedAt = newProduct.ProducedAt;
            product.ExpiredAt = newProduct.ExpiredAt;
            product.Description = newProduct.Description; 

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok(newProduct);
        }
        public async Task DeleteProduct(Product product, CancellationToken cancellationToken)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
