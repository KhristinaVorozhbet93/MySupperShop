﻿using System.Net.Http.Json;
using OnlineShop.HttpModels.Responses;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpApiClient.Extensions;
using System.Net.Http.Headers;

namespace OnlineShop.HttpApiClient
{
    public class MyShopClient : IMyShopClient, IDisposable
    {
        private readonly string _host;
        private readonly HttpClient? _httpClient;
        public bool IsAuthorizationTokenSet { get; private set; }
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
        public void SetAuthorizationToken(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            var header = new AuthenticationHeaderValue("Bearer", token);
            _httpClient!.DefaultRequestHeaders.Authorization = header;
            IsAuthorizationTokenSet = true;
        }

        //Product
        public async Task AddProduct(ProductRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var uri = "add_product";
            using var response = await _httpClient!
                .PostAsJsonAsync(uri, request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        public async Task<ProductResponse> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var uri = $"get_product?id={id}";
            var product = await _httpClient!
                .GetFromJsonAsync<ProductResponse>(uri, cancellationToken);
            if (product is null)
            {
                throw new InvalidOperationException(nameof(product));
            }
            return product;
        }
        public async Task<List<ProductResponse>> GetProducts(CancellationToken cancellationToken)
        {
            var uri = "get_products";
            var products = await _httpClient!
                .GetFromJsonAsync<List<ProductResponse>>(uri, cancellationToken);
            if (products is null)
            {
                throw new InvalidOperationException(nameof(products));
            }
            return products;
        }
        public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            var uri = "delete_product";
            using var response = await _httpClient!
                .PostAsJsonAsync(uri, id, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateProduct(ProductRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var uri = "update_product";
            await _httpClient!
                .PostAsJsonAsync(uri, request, cancellationToken);

            //using var response = await _httpClient!
            //    .PostAsJsonAsync("update_product", request, cancellationToken);
            //response.EnsureSuccessStatusCode();
        }

        //Account
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
            _httpClient!.DefaultRequestHeaders.Authorization
                 = new AuthenticationHeaderValue("Bearer", response.Token);
            return response;
        }
        public async Task<AccountResponse> GetAccount(CancellationToken cancellationToken)
        {
            var uri = "current";
            var response = await _httpClient!
                .GetFromJsonAsync<AccountResponse>(uri, cancellationToken);
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            return response;
        }

        public async Task UpdateAccountData(AccountRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var response = await _httpClient!.PostAsJsonAsync
                 ("account/data", request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateAccountPassword(AccountPasswordRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var response = await _httpClient!.PostAsJsonAsync
                ("account/password", request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
