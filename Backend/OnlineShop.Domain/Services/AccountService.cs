using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Exceptions;
using OnlineShop.WebApi;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationPasswordHasher _hasher;

        public AccountService(IUnitOfWork unitOfWork, IApplicationPasswordHasher hasher)
        {
            _hasher = hasher ?? throw new ArgumentException(nameof(hasher));
            _uow = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
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
            var existedAccount = await _uow.AccountRepozitory
                .FindAccountByLogin(login, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Account with this login alredy exist");
            }
            Account account = new Account
                (Guid.NewGuid(), login, EncryptPassword(password), email, roles);
            Cart cart = new(account.Id) { Id = Guid.NewGuid() };

            await _uow.AccountRepozitory.Add(account, cancellationToken); 
            await _uow.CartRepozitory.Add(cart, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return account;
        }

        private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            return hashedPassword;
        }
        public virtual async Task<Account> Login(string login,
            string password,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(password);

            var accountNotFound = await _uow.AccountRepozitory
                .FindAccountByLogin(login, cancellationToken);

            if (accountNotFound is null)
            {
                throw new AccountNotFoundException("Account with given login not found");
            }

            var account = await _uow.AccountRepozitory.GetAccountByLogin(login, cancellationToken);
            await CheckPassword(password, account, cancellationToken);
            return account;
        }

        public async Task<Account> GetAccountById(Guid guid,
            CancellationToken cancellationToken)
        {
            var account = await _uow.AccountRepozitory.GetById(guid, cancellationToken);
            return account;
        }

        public async Task<Account> GetAccountByLogin(string login,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);

            var accountNotFound = await _uow.AccountRepozitory
                                .FindAccountByLogin(login, cancellationToken);

            if (accountNotFound is null)
            {
                throw new AccountNotFoundException("Account with given login not found");
            }
            var account = await _uow.AccountRepozitory.GetAccountByLogin(login, cancellationToken);
            return account;
        }

        public async Task UpdateAccountData(string login, string name, string lastName,
            string email,
           CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(lastName);
            ArgumentNullException.ThrowIfNull(email);

            var account = await _uow.AccountRepozitory.GetAccountByLogin(login, cancellationToken);
            account.Name = name;
            account.LastName = lastName;
            account.Email = email;
            await _uow.AccountRepozitory.Update(account, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAccountPassword(string login, string oldPassword,
            string newPassword, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(oldPassword);
            ArgumentNullException.ThrowIfNull(newPassword);

            var account = await _uow.AccountRepozitory.GetAccountByLogin(login, cancellationToken);
            await CheckPassword(oldPassword, account, cancellationToken);
            account.HashedPassword = EncryptPassword(newPassword);
            await _uow.AccountRepozitory.Update(account, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        private async Task CheckPassword(string password, 
            Account account, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(account);
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
        }

        private async Task RehashPassword
            (string password, Account account, CancellationToken cancellationToken)
        {
            account.HashedPassword = EncryptPassword(password);
            await _uow.AccountRepozitory.Update(account, cancellationToken);
        }
    }
}
