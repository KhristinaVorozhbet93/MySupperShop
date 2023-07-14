using MySupperShop.Interfaces;
using MySupperShop.Models;
using System.Net.Http.Json;

namespace MySupperShop
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
                throw new ArgumentException("The host adress shoul be a valid URL",nameof(host));
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
        public async Task UpdateProduct(Guid id, Product newProduct, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(newProduct));
            using var response = await _httpClient!
                .PostAsJsonAsync($"update_product?id={id}", newProduct, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
