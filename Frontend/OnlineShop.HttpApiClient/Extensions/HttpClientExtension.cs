using OnlineShop.HttpApiClient.Exceptions;
using OnlineShop.HttpModels.Responses;
using System.Net;
using System.Net.Http.Json;

namespace OnlineShop.HttpApiClient.Extensions
{
    public static class HttpClientExtension
    {
        public static async Task<K> PostAndJsonDeserializeAsync<T, K>
            (this HttpClient client, string uri,
            T request, CancellationToken cancellationToken) 
        {
            ArgumentNullException.ThrowIfNull(nameof(client));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentException(nameof(uri));
            ArgumentNullException.ThrowIfNull(nameof(uri));
            ArgumentNullException.ThrowIfNull(nameof(request));
            var response = await client.PostAsJsonAsync(uri, request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new MyShopAPIException(error);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var details = await response.Content
                        .ReadFromJsonAsync<ValidationProblemDetails>();
                    throw new MyShopAPIException((int)response.StatusCode, details);
                }
                else
                {
                    throw new MyShopAPIException($"Неизвестная ошибка! {response.StatusCode}");
                }
            }
            var responseMessage =
                await response.Content.ReadFromJsonAsync<K>(cancellationToken: cancellationToken);
            return responseMessage!;
        }
    }
}
