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
        public IMyShopClient? ShopClient { get; set; }
        [Inject]
        public NavigationManager manager { get; set; }

        public string ProductFieldDeleted { get; set; } = string.Empty;
        private Product? _product;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        protected override async Task OnInitializedAsync()
        {
            _product = await ShopClient!.GetProduct(ProductId, _cts.Token);
        }
        public async Task DeleteProduct()
        {
            ProductFieldDeleted = string.Empty; 
            await ShopClient!.DeleteProduct(_product!, _cts.Token);
            ProductFieldDeleted = "Товар удален!";
            await InvokeAsync(() => StateHasChanged());
            await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token); 
            manager.NavigateTo("/catalog");
        }

        public void ToProductEditPage()
        {
            manager.NavigateTo($"/products/{_product!.Id}/editor");
        }

        public void ToAddProductPage()
        {
            manager.NavigateTo($"/products/new");
        }
        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}