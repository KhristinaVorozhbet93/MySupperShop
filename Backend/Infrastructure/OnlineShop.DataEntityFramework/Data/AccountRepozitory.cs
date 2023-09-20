using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Data
{
    public class AccountRepozitory : EfRepozitory<Account>, IAccountRepozitory
    {
        public AccountRepozitory(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Account> GetAccountByLogin(string login, CancellationToken cancellationToken)
        {
            if (login is null)
            {
                throw new ArgumentException(nameof(login));
            }
            return await _entities.SingleAsync(e => e.Login == login, cancellationToken);
        }
        public async Task<Account?> FindAccountByLogin(string login, CancellationToken cancellationToken)
        {
            if (login is null)
            {
                throw new ArgumentException(nameof(login));
            }
            return await _entities.SingleOrDefaultAsync(e => e.Login == login, cancellationToken);
        }
    }
}
