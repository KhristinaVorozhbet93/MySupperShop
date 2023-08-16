using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpApiClient.Data;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentException(nameof(accountService));
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
                return new LoginResponse(account.Id, account.Login);
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
    }
}
