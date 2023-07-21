using MyShopBackend.Models;

namespace MyShopBackend.Interfaces
{
    public interface IAccountRepozitory : IRepozitory<Account>
    {
        Task<Account> GetByEmail(string email, CancellationToken cancellationToken);
    }
}
