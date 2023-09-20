using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using OnlineShop.WebApi.Filtres;
using OnlineShop.WebApi.Services;
using System.Security.Claims;

namespace OnlineShop.WebApi.Controllers
{
    [CentralizedExceptionHandlingFilter]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(AccountService accountService, 
            ITokenService tokenService)
        {
            _accountService = accountService ?? throw new ArgumentException(nameof(accountService));
            _tokenService = tokenService ?? throw new ArgumentException(nameof(tokenService));
        }

        [HttpPost("account/registration")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request,
           CancellationToken cancellationToken)
        {
            var role = new Role[] { Role.Admin };
            var account = await _accountService.Register
                (request.Login, request.Password, request.Email, role, cancellationToken);
            return new RegisterResponse(account.Login);
        }

        [HttpPost("account/login_by_password")]
        public async Task<ActionResult<LoginResponse>> LoginByPassword
            (LoginRequest request, CancellationToken cancellationToken)
        {
            var (account,codeId) =
                await _accountService.Login(request.Login, request.Password, cancellationToken);                     
            return new LoginResponse(account.Id, account.Login, codeId);
        }

        [HttpPost("account/login_by_code")]
        public async Task<ActionResult<LoginByCodeResponse>> LoginByCode
           (LoginByCodeRequest request, CancellationToken cancellationToken)
        {
            var account =
                await _accountService.LoginByCode
                (request.Login,request.CodeId, request.Code, cancellationToken);
            var token = _tokenService.GenerateToken(account);
            return new LoginByCodeResponse(account.Id, account.Login, token); 
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<AccountResponse>> GetCurrentAccount
            (CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(strId);

            var account = await _accountService.GetAccountById(userId, cancellationToken);
            return new AccountResponse
                (account.Id, account.Login, account.Email,
                account.Name, account.LastName, account.Image);
        }

        [Authorize]
        [HttpPost("account/data")]
        public async Task<ActionResult> UpdateAccount
            (AccountRequest request, CancellationToken cancellationToken)
        {
            await _accountService.UpdateAccountData(request.Login, request.Name,
                request.LastName, request.Email, request.Image, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("account/password")]
        public async Task<ActionResult> UpdateAccountPassword
            (AccountPasswordRequest request, CancellationToken cancellationToken)
        {
            await _accountService.UpdateAccountPassword
                (request.Login, request.OldPassword, request.NewPassword, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("account/delete")]
        public async Task<ActionResult> DeleteAccount(AccountRequest request,
            CancellationToken cancellationToken)
        {
            await _accountService.DeleteAccount(request.Login, cancellationToken);
            return Ok();
        }
    }
}
