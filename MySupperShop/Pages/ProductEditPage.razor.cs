using Microsoft.AspNetCore.Components;
using MySupperShop.Interfaces;
using MySupperShop.Models;

namespace MySupperShop.Pages {
    public partial class ProductEditPage : IDisposable
    {
        [Parameter]
        public Guid ProductId { get; set; }
        [Inject]
        public IMyShopClient? ShopClient { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string Image { get; set; } = string.Empty;
        public DateTime ProducedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ProductFieldChanged{ get; set; } = "";

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
                ProductFieldChanged = "Некорректно введено имя!";
                return;
            }
            if (Price <= 0)
            {
                ProductFieldChanged = "Некорректно введена цена!";
                return;
            }
            var newProduct = new Product(Name, Price)
            {
                Id = _product!.Id,
                ProducedAt = ProducedAt,
                ExpiredAt = ExpiredAt,
                Description = Description
            };
            await ShopClient!.UpdateProduct(newProduct, _cts.Token);
            ProductFieldChanged = "Товар изменен!";
            await InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}