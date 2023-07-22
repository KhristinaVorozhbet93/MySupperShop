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

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request, 
            CancellationToken cancellationToken)
        {
            Account account = new Account(Guid.Empty, request.Login, request.Password, request.Email);
            var existedAccount = await _accountRepozitory.FindAccountByEmail(request.Email, cancellationToken);
            if (existedAccount is not null)
            {
                return BadRequest("Account with this email alredy exist");
            }
            await _repozitory.Add(account, cancellationToken);
            return Ok();
        }
        [HttpGet("get_account_by_id")]
        public async Task<ActionResult> GetAccountById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _repozitory.GetById(id, cancellationToken);
                return Ok(account);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        [HttpGet("get_accounts")]
        public async Task<ActionResult<Account>> GetAccounts(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _repozitory.GetAll(cancellationToken);
                return Ok(accounts);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        [HttpGet("get_account_by_email")]
        public async Task<ActionResult<Account>> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepozitory.GetAccountByEmail(email, cancellationToken);
                return Ok(account);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
