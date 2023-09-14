using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShop.Domain.Exceptions;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.WebApi.Filtres
{
    public class CentralizedExceptionHandlingFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var message = TryGetUserMessageFromException(context);
            int statusCode = StatusCodes.Status409Conflict;

            if (message != null)
            {
                context.Result = new ObjectResult(new ErrorResponse(message, statusCode))
                {
                    StatusCode = statusCode
                };
                context.ExceptionHandled = true; 
            }
        }

        private string? TryGetUserMessageFromException(ExceptionContext context)
        {
            return context.Exception switch
            {
                EmailAlreadyExistsException => "Аккаунт с таким login уже зарегистрирован!",
                AccountNotFoundException => "Аккаунт с таким логином не найден!",
                InvalidPasswordException => "Неверный пароль!",
                ProductNotFoundException => "Не найден продукт с данным id!",
                CartNotFoundException => "Корзины по такому id не сущестует!",
                DomainException => "Неизвестная ошибка", 
                _ => null
            }; 
        }
    }
}
