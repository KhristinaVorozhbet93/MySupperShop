using Microsoft.AspNetCore.Components;
using MySupperShop.Interfaces;
using MySupperShop.Models;

namespace MySupperShop.Pages
{
    public partial class ProductPage : IDisposable
    {
        [Parameter]
        public Guid ProductId { get; set; }

        [Inject]
        public IMyShopClient ShopClient { get; set; }

        private Product _product;

        CancellationTokenSource _cts = new CancellationTokenSource();

        protected override async Task OnInitializedAsync()
        {
            _product = await ShopClient.GetProduct(ProductId, _cts.Token);
        }
        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}