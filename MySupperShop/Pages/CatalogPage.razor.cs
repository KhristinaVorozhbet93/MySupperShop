using MySupperShop.Models;
using Microsoft.AspNetCore.Components;
using MySupperShopHttpApiClient.Interfaces;

namespace MySupperShop.Pages
{
    public partial class CatalogPage : IDisposable
    {
        [Inject]
        private IMyShopClient? ShopClient { get; set; }
        [Inject]
        public NavigationManager manager { get; set; }
        private List<Product>? _products;
        private CancellationTokenSource _cts = new();

        protected override async Task OnInitializedAsync()
        {
            _products = await ShopClient!.GetProducts(_cts.Token);
        }
        public void Dispose()
        {
            _cts.Cancel();
        }
        public void ToAddProductPage()
        {
            manager.NavigateTo($"/products/new");
        }
    }
}
