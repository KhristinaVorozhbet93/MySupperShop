using OnlineShop.HttpModels.Responses;
using System.Net;

namespace OnlineShop.HttpApiClient.Exceptions
{
    [Serializable]
    public class MyShopAPIException : Exception
    {
        public ErrorResponse Error { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public ValidationProblemDetails? Details { get; set; }

        public MyShopAPIException()
        {
        }

        public MyShopAPIException(HttpStatusCode statusCode, ValidationProblemDetails details)
            : base(details.Title)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public MyShopAPIException(ErrorResponse? error) : base(error.Message)
        {
            Error = error;
            StatusCode = error.StatusCode;
        }


        public MyShopAPIException(string? message) : base(message)
        {
        }

        public MyShopAPIException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}