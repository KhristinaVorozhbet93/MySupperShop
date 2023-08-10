using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpApiClient.Entities;

namespace OnlineShop.Frontend.Pages
{
    public partial class AddProductPage : IDisposable
    {
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
        [Inject]
        public IDialogService DialogService { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public DateTime ProducedAt { get; set; } = DateTime.Now;
        public DateTime ExpiredAt { get; set; }
        public string Description { get; set; } = string.Empty;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public void Dispose()
        {
            _cts.Cancel();
        }
        public async Task SaveProductChanges()
        {
            if (Name == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введено имя!");
                return;
            }
            if (Price <= 0)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введена цена!");
                return;
            }
            var newProduct = new Product(Name, Price)
            {
                Id = Guid.NewGuid(),
                ProducedAt = ProducedAt,
                ExpiredAt = ExpiredAt,
                Description = Description,
                Image = Image
            };
            try
            {
                await ShopClient!.AddProduct(newProduct, _cts.Token);
                await DialogService.ShowMessageBox("Успешно", "Товар добавлен!");
            }
            catch (ArgumentNullException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Товар не добавлен!");
            }
        }
    }
}