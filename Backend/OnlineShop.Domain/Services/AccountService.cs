using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Events;
using MediatR;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationPasswordHasher _hasher;
        private readonly IMediator _mediator;

        public AccountService(IUnitOfWork unitOfWork, 
            IApplicationPasswordHasher hasher, 
            IMediator mediator)
        {
            _hasher = hasher ?? throw new ArgumentException(nameof(hasher));
            _uow = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

            await _mediator.Publish(new AccountRegistredEvent(account), cancellationToken);
            return account;
        }

        public virtual async Task<(Account account, Guid codeId)> Login(string login,
            string password,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(password);
            Account account = await LoginByPassword(login, password, cancellationToken);
            var code = await CreateAndSendConfirmationCode(account,cancellationToken);
            return (account, code.Id);
        }

        public virtual async Task<Account> LoginByCode(string login, Guid codeId,
            string code,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(code);
            ArgumentNullException.ThrowIfNull(login);

            var confirmationCode = await _uow.ConfirmationCodeRepozitory.GetById
                (codeId, cancellationToken);

            if (confirmationCode is null)
            {
                throw new CodeNotFoundException("Code not found");
            }
            if (confirmationCode.Code != code)
            {
                throw new InvalidCodeException("Invalid code");
            }

            var account = await _uow.AccountRepozitory
                .GetAccountByLogin(login, cancellationToken);
            return account;      
        }

        private async Task<Account> LoginByPassword(string login, string password, CancellationToken cancellationToken)
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
            string email,string? image,
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
            account.Image = image; 
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
            var isPasswordValid = _hasher.VerifyHashedPassword
                         (account.HashedPassword, oldPassword, out var rehashNedded);

            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password");
            }
            if (rehashNedded)
            {
                await RehashPassword(oldPassword, account, cancellationToken);
            }

            account.HashedPassword = EncryptPassword(newPassword);
            await _uow.AccountRepozitory.Update(account, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAccount(string login, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(login);
            var account = await _uow.AccountRepozitory.GetAccountByLogin(login, cancellationToken);
            await _uow.AccountRepozitory.Delete(account, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            return hashedPassword;
        }

        private async Task RehashPassword
            (string password, Account account, CancellationToken cancellationToken)
        {
            account.HashedPassword = EncryptPassword(password);
            await _uow.AccountRepozitory.Update(account, cancellationToken);
        }

        private async Task<ConfirmationCode> CreateAndSendConfirmationCode(Account account,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(account));
            ConfirmationCode code = GenerateNewConfirmationCode(account);
            await _uow.ConfirmationCodeRepozitory.Add(code, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new UserLoginedByPasswordEvent(account.Email, "Подтверждение кода",
                $"Код подтверждения {code.Code}"), cancellationToken);
            return code; 
        }

        public ConfirmationCode GenerateNewConfirmationCode(Account account)
        {
            ArgumentNullException.ThrowIfNull(nameof(account));
            return new ConfirmationCode(Guid.NewGuid(), account.Id, 
                DateTime.Now, TimeSpan.FromSeconds(10));
        }
    }
}
