using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class CatalogPage 
    {
        [Inject] private IMyShopClient ShopClient { get; set; }
        [Inject] private NavigationManager Manager { get; set; }
        private List<ProductResponse>? _products;
        private CancellationTokenSource _cts = new();
        private bool _catalogLoading;

        protected override async Task OnInitializedAsync()
        {
            _catalogLoading = true;
            _products = await ShopClient.GetProducts(_cts.Token);
            _catalogLoading = false;
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
