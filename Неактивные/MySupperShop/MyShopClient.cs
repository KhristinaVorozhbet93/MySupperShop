using MySupperShop.Interfaces;
using MySupperShop.Models;
using System.Net.Http.Json;

namespace MySupperShop
{
    //сделать CRUD
    //добавить товары в бд 
    //реализовать Dispoose что он там хотел 
    //внедрить зависимость
    //* Выведите список товаров на странице:
    //Подсказка: перезапишите метод OnInitializedAsync. Добавьте CORS
    //

    public class MyShopClient : IMyShopClient
    {
        private readonly string _host;
        private readonly HttpClient? _httpClient;
        public MyShopClient(string host = "http://myshop.com/", HttpClient? httpClient = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(host);
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
        public async Task AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));
            var response = await _httpClient.PostAsJsonAsync("", product);
            response.EnsureSuccessStatusCode();
        }
        public async Task<Product> GetProduct(Guid id)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"get_product?id={id}");
            if (product is null)
            {
                throw new InvalidOperationException(nameof(product)); 
            }
            return product; 
        }

        //public async Task<Product> GetProducts()
        //{
        //    var products = await _httpClient.GetFromJsonAsync<Product>("");

        //    if (products is null)
        //    {
        //        throw new InvalidOperationException(nameof(product));
        //    }
        //    return products;
        //}


        public async Task DeleteProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));

            //убедиться что код ответа правильный
        }
    }
}
