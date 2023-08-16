using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces
{
    public interface IAccountRepozitory : IRepozitory<Account>
    {
        Task<Account> GetAccountByLogin(string login, CancellationToken cancellationToken);
        Task<Account?> FindAccountByLogin(string login, CancellationToken cancellationToken);
    }
}
