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
        public async Task<Account> GetAccountById([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _repozitory.GetById(id, cancellationToken);
        }
        [HttpGet("get_account_by_email")]
        public async Task<Account> GetAccountByEmail([FromQuery] string email, CancellationToken cancellationToken)
        {
            return await _accountRepozitory.GetByEmail(email, cancellationToken);
        }
    }
}
