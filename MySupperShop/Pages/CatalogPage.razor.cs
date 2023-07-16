using MySupperShop.Models;
using MySupperShop.Interfaces;
using Microsoft.AspNetCore.Components;

namespace MySupperShop.Pages
{
    public partial class CatalogPage : IDisposable
    {
        [Inject]
        private IMyShopClient? ShopClient { get; set; }
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
    }
}
