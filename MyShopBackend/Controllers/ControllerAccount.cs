using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend.Controllers
{
    public class ControllerAccount : ControllerBase
    {
        private readonly IRepozitory<Account> _repozitory;
        private readonly IAccountRepozitory _accountRepozitory;

        public ControllerAccount(IRepozitory<Account> repozitory, IAccountRepozitory accountRepozitory)
        {
            _repozitory = repozitory ?? throw new ArgumentException(nameof(repozitory));
            _accountRepozitory = accountRepozitory ?? throw new ArgumentException(nameof(accountRepozitory));
        }

        [HttpPost("add_account")]
        public async Task AddAccount([FromBody] Account account, CancellationToken cancellationToken)
        {
            await _repozitory.Add(account, cancellationToken);
        }
        [HttpGet("get_account_by_id")]
        public async Task<IResult> GetAccountById([FromQuery] Guid id, CancellationToken cancellationToken)
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
        [HttpGet("get_account_by_email")]
        public async Task<IResult> GetAccountByEmail([FromQuery] string email, CancellationToken cancellationToken)
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
