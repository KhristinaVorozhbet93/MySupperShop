using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Exceptions;
using OnlineShop.WebApi;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IAccountRepozitory _accountRepozitory;
        private readonly IRepozitory<Account> _repozitory;
        private readonly IApplicationPasswordHasher _hasher;

        public AccountService(IAccountRepozitory accountRepozitory,
            IRepozitory<Account> repozitory,
            IApplicationPasswordHasher hasher)
        {
            _accountRepozitory =
                accountRepozitory ?? throw new ArgumentException(nameof(accountRepozitory));
            _repozitory = repozitory ?? throw new ArgumentException(nameof(repozitory));
            _hasher = hasher ?? throw new ArgumentException(nameof(hasher));

        }
        public virtual async Task<Account> Register(string login, 
            string password,
            string email,
            Role[] roles,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);
            var existedAccount = await _accountRepozitory
                .FindAccountByLogin(login, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Account with this login alredy exist");
            }
            Account account = new Account(Guid.Empty, login, EncryptPassword(password), email, roles);
            await _repozitory.Add(account, cancellationToken);
            return account; 
        }

        private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            Console.WriteLine(hashedPassword);
            Console.WriteLine(password);
            return hashedPassword;
        }
        public virtual async Task<Account> Login(string login, 
            string password, 
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(password);

            var accountNotFound = await _accountRepozitory
                .FindAccountByLogin(login, cancellationToken);

            if (accountNotFound is null)
            {
                throw new AccountNotFoundException("Account with given login not found");
            }

            var account = await _accountRepozitory.GetAccountByLogin(login, cancellationToken);

            var isPasswordValid = _hasher.VerifyHashedPassword
                (account.HashedPassword, password, out var rehashNedded);

            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            if (rehashNedded)
            {
                await RehashPassword(password, account, cancellationToken);
            }
            return account;
        }
        public async Task <Account> GetAccountById(Guid guid,
            CancellationToken cancellationToken)
        {
            return  await _repozitory.GetById(guid,cancellationToken);
        }

        private async Task RehashPassword
            (string password, Account account, CancellationToken cancellationToken)
        {
            account.HashedPassword = EncryptPassword(password);
            await _accountRepozitory.Update(account, cancellationToken);
        }
    }
}
