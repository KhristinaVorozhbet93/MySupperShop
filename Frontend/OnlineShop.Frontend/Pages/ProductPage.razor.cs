using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class ProductPage : IDisposable
    {
        [Parameter] public Guid ProductId { get; set; }
        [Inject] private IMyShopClient ShopClient { get; set; }
        [Inject] private NavigationManager manager { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        string state = string.Empty;
        private ProductResponse? _product;
        private CancellationTokenSource _cts = new();

        public void Dispose()
        {
            _cts.Cancel();
        }
        protected override async Task OnInitializedAsync()
        {
            _product = await ShopClient!.GetProduct(ProductId, _cts.Token);
        }
        public async Task DeleteProduct()
        {
            try
            {
                bool? result = await DialogService.ShowMessageBox(
            "Информация",
            "Вы верены, что хотите удалить продукт?",
            yesText: "Да", cancelText: "Нет");
                state = result == null ? "No" : "Yes";
                if (state == "Yes")
                {
                    await ShopClient!.DeleteProduct(ProductId, _cts.Token);
                    await DialogService.ShowMessageBox("Информация", "Товар удален!");
                    await Task.Delay(TimeSpan.FromSeconds(3), _cts.Token);
                    manager.NavigateTo("/catalog");
                }
                if (state == "No")
                {
                    await DialogService.ShowMessageBox("Информация", "Товар не удален!");
                    return;
                }
            }
            catch (ArgumentNullException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Товар не удален!");
            }
        }
        public void ToProductEditPage()
        {
            manager.NavigateTo($"/products/{_product!.Id}/editor");
        }
    }
}