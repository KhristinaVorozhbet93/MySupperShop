using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShop.WebApi.Filtres
{
    public class AuthorizationAPIKeyFilter : Attribute, IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationAPIKeyFilter> _logger;

        public AuthorizationAPIKeyFilter(ILogger<AuthorizationAPIKeyFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var key = httpContext.Request.Headers["Api-Key"].ToString();

            if (key != "API_KEY")
            {
                _logger.LogInformation("Неверный ключ авторизации!");
                context.Result =
                    new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            _logger.LogInformation("Ключи совпадают!");
        }
    }
}
