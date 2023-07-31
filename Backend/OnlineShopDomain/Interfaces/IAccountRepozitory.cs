using MyShopBackend.Models;

namespace MyShopBackend.Interfaces
{
    public interface IAccountRepozitory : IRepozitory<Account>
    {
        Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken);
        Task<Account> FindAccountByLogin(string login, CancellationToken cancellationToken);
    }
}
