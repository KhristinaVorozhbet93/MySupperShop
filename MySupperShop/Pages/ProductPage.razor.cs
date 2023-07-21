using Microsoft.AspNetCore.Components;
using MudBlazor;
using MySupperShop.Models;
using MySupperShopHttpApiClient.Interfaces;

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
        [Inject]
        public IDialogService DialogService { get; set; }
        string state = string.Empty;
        private Product? _product;
        private CancellationTokenSource _cts = new CancellationTokenSource();

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
                    await ShopClient!.DeleteProduct(_product!, _cts.Token);
                    await DialogService.ShowMessageBox("Информация", "Товар удален!");
                    await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
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
        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}