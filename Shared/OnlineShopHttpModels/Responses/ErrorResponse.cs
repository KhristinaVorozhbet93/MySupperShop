using System.Net;

namespace OnlineShopHttpModels.Responses
{
    public record ErrorResponse (string Message, HttpStatusCode? StatusCode = null);
}
