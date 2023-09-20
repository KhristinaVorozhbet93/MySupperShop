using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
namespace OnlineShop.Data.EntityFramework.Data
{
    public class UnitOfWorkEf : IUnitOfWork
    {
        public IAccountRepozitory AccountRepozitory { get; }
        public ICartRepozitory CartRepozitory { get; }
        public IProductRepozitory ProductRepozitory { get; }
        public IConfirmationCodeRepozitory ConfirmationCodeRepozitory { get; }
        private readonly AppDbContext _dbContext;

        public UnitOfWorkEf(
        AppDbContext dbContext,
            IAccountRepozitory accountRepository,
            ICartRepozitory cartRepository, 
            IProductRepozitory productRepozitory, 
            IConfirmationCodeRepozitory confirmationCodeRepozitory)

        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            AccountRepozitory = accountRepository 
                ?? throw new ArgumentNullException(nameof(accountRepository));
            CartRepozitory = cartRepository 
                ?? throw new ArgumentNullException(nameof(cartRepository));
            ProductRepozitory = productRepozitory
                ?? throw new ArgumentNullException(nameof(productRepozitory));
            ConfirmationCodeRepozitory = confirmationCodeRepozitory
                ?? throw new ArgumentNullException(nameof(confirmationCodeRepozitory));
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
