using System.Net;

namespace OnlineShop.HttpModels.Responses
{
    public record ErrorResponse(string Message, HttpStatusCode? StatusCode = null);
}
