using Microsoft.AspNetCore.Components;
using MudBlazor;
using MySupperShop.Models;
using MySupperShopHttpApiClient.Interfaces;

namespace MySupperShop.Pages
{
    public partial class ProductEditPage : IDisposable
    {
        [Parameter]
        public Guid ProductId { get; set; }
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
        [Inject]
        public IDialogService DialogService { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string Image { get; set; } = string.Empty;
        public DateTime ProducedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Description { get; set; } = string.Empty;
        private Product? _product;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        protected override async Task OnInitializedAsync()
        {
            _product = await ShopClient!.GetProduct(ProductId, _cts.Token);
            Name = _product.Name;
            Price = _product.Price;
            Image = _product.Image;
            ProducedAt = _product.ProducedAt;
            ExpiredAt = _product.ExpiredAt;
            Description = _product.Description;
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
            _product!.Name = Name;
            _product.Price = Price;
            _product.Image = Image;
            _product.ProducedAt = ProducedAt;
            _product.ExpiredAt = ExpiredAt;
            _product.Description = Description;
            try
            {
                await ShopClient!.UpdateProduct(_product, _cts.Token);
                await DialogService.ShowMessageBox("Успешно", "Товар изменен!");
            }
            catch (ArgumentNullException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Товар не изменен!");
            }
        }
        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}