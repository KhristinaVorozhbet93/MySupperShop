using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Requests;

namespace OnlineShop.Frontend.Pages
{
    public partial class AddProductPage : IDisposable
    {
        [Inject] private IMyShopClient ShopClient { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        private CancellationTokenSource _cts = new();

        private ProductRequest model = new();
        public void Dispose()
        {
            _cts.Cancel();
        }
        public async Task SaveProductChanges()
        {
            if (model.Name == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введено имя!");
                return;
            }
            if (model.Description == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введено описание товара!");
                return;
            }
            if (model.Price <= 0)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введена цена!");
                return;
            }
            if (model.Image == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введен путь к изображению!");
                return;
            }

            try
            {
                await ShopClient!.AddProduct(model, _cts.Token);
                await DialogService.ShowMessageBox("Успешно", "Товар добавлен!");
            }
            catch (ArgumentNullException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Товар не добавлен!");
            }
        }
    }
}