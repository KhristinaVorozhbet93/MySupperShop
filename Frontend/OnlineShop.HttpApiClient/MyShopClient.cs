using System.Net.Http.Json;
using OnlineShop.HttpModels.Responses;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpApiClient.Data;
using OnlineShop.HttpApiClient.Entities;
using OnlineShop.HttpApiClient.Extensions;

namespace OnlineShop.HttpApiClient
{
    public class MyShopClient : IMyShopClient, IDisposable
    {
        private readonly string _host;
        private readonly HttpClient? _httpClient;
        public MyShopClient(string host = "http://myshop.com/", HttpClient? httpClient = null)
        {
            ArgumentNullException.ThrowIfNull(host);
            if (string.IsNullOrEmpty(host)) throw new ArgumentException(nameof(host));

            if (!Uri.TryCreate(host, UriKind.Absolute, out var hostUri))
            {
                throw new ArgumentException("The host adress shoul be a valid URL", nameof(host));
            }
            _host = host;
            _httpClient = httpClient ?? new HttpClient();
            if (_httpClient.BaseAddress is null)
            {
                _httpClient.BaseAddress = hostUri;
            }
        }
        public void Dispose()
        {
            _httpClient!.Dispose();
        }
        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));
            using var response = await _httpClient!
                .PostAsJsonAsync("add_product", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        public async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var product = await _httpClient!
                .GetFromJsonAsync<Product>($"get_product?id={id}", cancellationToken);
            if (product is null)
            {
                throw new InvalidOperationException(nameof(product));
            }
            return product;
        }
        public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _httpClient!
                .GetFromJsonAsync<List<Product>>("get_products", cancellationToken);
            if (products is null)
            {
                throw new InvalidOperationException(nameof(products));
            }
            return products;
        }
        public async Task DeleteProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));
            using var response = await _httpClient!
                .PostAsJsonAsync("delete_product", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));
            await _httpClient!
                .PostAsJsonAsync($"update_product", product, cancellationToken);
        }
        public async Task<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var uri = "account/registration";
            var response = await _httpClient!
                .PostAndJsonDeserializeAsync<RegisterRequest, RegisterResponse>
               (uri, request, cancellationToken);
            return response;
        }
        public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var uri = "account/login";
            var response = await _httpClient!
                .PostAndJsonDeserializeAsync<LoginRequest, LoginResponse>
                (uri, request, cancellationToken);
            return response;
        }
    }
}
