using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Data
{
    public class CartRepozitory : EfRepozitory<Cart>, ICartRepozitory
    {
        private readonly AppDbContext _dbContext;
        public CartRepozitory(AppDbContext _dbContext) : base(_dbContext) { }

        public async Task<Cart> GetCartByAccountId(Guid id, CancellationToken cancellationToken)
        {
            return await _entities.SingleAsync(e => e.AccountId == id, cancellationToken);
        }
    }
}
