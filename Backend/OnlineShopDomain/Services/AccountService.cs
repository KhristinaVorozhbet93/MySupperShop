using MyShopBackend.Interfaces;
using MyShopBackend.Models;
using OnlineShopDomain.Exxceptions;

namespace MyShopBackend.Services
{
    public class AccountService
    {
        private readonly IAccountRepozitory _accountRepozitory;
        private readonly IRepozitory<Account> _repozitory;

        public AccountService(IAccountRepozitory accountRepozitory, IRepozitory<Account> repozitory)
        {
            _accountRepozitory = 
                accountRepozitory ?? throw new ArgumentException(nameof(accountRepozitory));
            _repozitory = repozitory ?? throw new ArgumentException(nameof(repozitory));
        }
        public async Task Register(string login, string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);
            var existedAccount = await _accountRepozitory.FindAccountByLogin(login, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Account with this login alredy exist");
            }
            Account account = new Account(Guid.Empty,login, password, email);
            await _repozitory.Add(account, cancellationToken);
        }
    }
}
