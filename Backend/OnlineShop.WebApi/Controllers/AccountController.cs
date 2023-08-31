using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using OnlineShop.WebApi.Services;
using System.Security.Claims;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(AccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService ?? throw new ArgumentException(nameof(accountService));
            _tokenService = tokenService ?? throw new ArgumentException(nameof(tokenService));
        }

        [HttpPost("account/registration")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request,
           CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountService.Register
                    (request.Login, request.Password, request.Email, cancellationToken);
                return new RegisterResponse(account.Login);
            }
            catch (EmailAlreadyExistsException)
            {
                return Conflict(new ErrorResponse("Аккаунт с таким login уже зарегистрирован!"));
            }
        }

        [HttpPost("account/login")]
        public async Task<ActionResult<LoginResponse>> Login
            (LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var account =
                    await _accountService.Login(request.Login, request.Password, cancellationToken);
                var token = _tokenService.GenerateToken(account);
                return new LoginResponse(account.Id, account.Login, token);
            }
            catch (AccountNotFoundException)
            {
                return Conflict(new ErrorResponse("Аккаунт с таким логином не найден!"));
            }
            catch (InvalidPasswordException)
            {
                return Conflict(new ErrorResponse("Неверный пароль!"));
            }
        }

        [Authorize]
        [HttpPost("current")]
        public async Task<ActionResult<AccountResponse>> GetCurrentAccount
            (CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(strId);

            var account = await _accountService.GetAccountById(userId, cancellationToken);
            return new AccountResponse(account.Id, account.Login, account.Email);
        }
    }
}
