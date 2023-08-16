using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpApiClient.Entities;

namespace OnlineShop.Frontend.Pages
{
    public partial class CatalogPage : IDisposable
    {
        [Inject]
        private IMyShopClient? ShopClient { get; set; }
        [Inject]
        public NavigationManager Manager { get; set; }
        private List<Product>? _products;
        private CancellationTokenSource _cts = new();
        private bool _catalogLoading;
        public void Dispose()
        {
            _cts.Cancel();
        }
        protected override async Task OnInitializedAsync()
        {
            _catalogLoading = true;
            _products = await ShopClient!.GetProducts(_cts.Token);
            _catalogLoading = false; ;
        }
        public void ToAddProductPage()
        {
            Manager.NavigateTo("/products/new");
        }
        public void ToIndexPage()
        {
            Manager.NavigateTo("/");
        }
    }
}
