using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend.Data
{
    public class ProductRepozitory : EfRepozitory<Product>, IProductRepozitory
    {
        private readonly AppDbContext _dbContext;

        public ProductRepozitory(AppDbContext _dbContext) : base(_dbContext) { }
    }
}
