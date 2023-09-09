using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Data
{
    public class CartRepozitory : EfRepozitory<Cart>, ICartRepozitory
    {
        public CartRepozitory(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Cart> GetCartByAccountId(Guid accountId, CancellationToken cancellationToken)
        {
            var account = await _entities
                .SingleOrDefaultAsync(it => it.AccountId == accountId, cancellationToken) 
                ?? throw new AccountNotFoundException("Account with given email not found");
            var cart = await _entities
                 .Include(c => c.Items)
                 .SingleOrDefaultAsync(e => e.AccountId == accountId, cancellationToken);
            return cart;
        }
    }
}
