using MyShopBackend.Models;

namespace MyShopBackend.Interfaces
{
    public interface IAccountRepozitory : IRepozitory<Account>
    {
        Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken);
        Task<Account> FindAccountByEmail(string email, CancellationToken cancellationToken);
    }
}
