using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Data
{
    public class AccountRepozitory : EfRepozitory<Account>, IAccountRepozitory
    {
        private readonly AppDbContext _dbContext;
        public AccountRepozitory(AppDbContext _dbContext) : base(_dbContext) { }

        public async Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
            {
                throw new ArgumentException(nameof(email));
            }
            return await _entities.SingleAsync(e => e.Email == email, cancellationToken);
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
