using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class ProductEditPage : IDisposable
    {
        [Parameter] public Guid ProductId { get; set; }
        [Inject] private IMyShopClient ShopClient { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        private ProductRequest model = new();
        private ProductResponse? _product;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public void Dispose()
        {
            _cts.Cancel();
        }
        protected override async Task OnInitializedAsync()
        {
            _product = await ShopClient!.GetProduct(ProductId, _cts.Token); 
            model.Name = _product.Name;
            model.Price = _product.Price;
            model.Image = _product.Image;
            model.ProducedAt = _product.ProducedAt;
            model.ExpiredAt = _product.ExpiredAt;
            model.Description = _product.Description;
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
                await ShopClient!.UpdateProduct(model, _cts.Token);
                await DialogService.ShowMessageBox("Успешно", "Товар изменен!");
            }
            catch (ArgumentNullException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Товар не изменен!");
            }
        }
    }
}