using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepozitory<Account> _repozitory;
        private readonly IAccountRepozitory _accountRepozitory;

        public AccountController(IRepozitory<Account> repozitory, IAccountRepozitory accountRepozitory)
        {
            _repozitory = repozitory ?? throw new ArgumentException(nameof(repozitory));
            _accountRepozitory = accountRepozitory ?? throw new ArgumentException(nameof(accountRepozitory));
        }

        [HttpPost("add_account")]
        public async Task AddAccount(Account account, CancellationToken cancellationToken)
        {
            await _repozitory.Add(account, cancellationToken);
        }
        [HttpGet("get_account_by_id")]
        public async Task<IResult> GetAccountById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _repozitory.GetById(id, cancellationToken);
                return Results.Ok(account);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        }
        [HttpGet("get_accounts")]
        public async Task<IResult> GetAccounts(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _repozitory.GetAll(cancellationToken);
                return Results.Ok(accounts);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        }
        [HttpGet("get_account_by_email")]
        public async Task<IResult> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepozitory.GetByEmail(email, cancellationToken);
                return Results.Ok(account);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        }
    }
}
