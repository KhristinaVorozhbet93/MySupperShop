using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;
using MyShopBackend.Services;
using OnlineShopDomain.Exxceptions;
using OnlineShopHttpModels.Responses;

namespace MyShopBackend.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepozitory<Account> _repozitory;
        private readonly IAccountRepozitory _accountRepozitory;
        private readonly AccountService _accountService;

        public AccountController(IRepozitory<Account> repozitory,
            IAccountRepozitory accountRepozitory,
            AccountService accountService)
        {
            _repozitory = repozitory ?? throw new ArgumentException(nameof(repozitory));
            _accountRepozitory = accountRepozitory ?? throw new ArgumentException(nameof(accountRepozitory));
            _accountService = accountService ?? throw new ArgumentException(nameof(accountService));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _accountService.Register
                    (request.Login, request.Password, request.Email, cancellationToken);
                return Ok();
            }
            catch (EmailAlreadyExistsException)
            {
                return Conflict(new ErrorResponse("Аккаунт с таким email уже зарегистрирован!"));
            }      
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
        public async Task<ActionResult<Account>> GetAccountByEmail
            (string email, CancellationToken cancellationToken)
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
