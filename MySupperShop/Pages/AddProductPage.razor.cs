using Microsoft.AspNetCore.Components;
using MySupperShop.Interfaces;
using MySupperShop.Models;

namespace MySupperShop.Pages
{
    public partial class AddProductPage : IDisposable
    {
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public DateTime ProducedAt { get; set; } = DateTime.Now;
        public DateTime ExpiredAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ProductFieldAdded { get; set; } = "";

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public async Task SaveProductChanges()
        {
            if (Name == string.Empty)
            {
                ProductFieldAdded = "Некорректно введено имя!";
                return; 
            }
            if(Price <= 0)
            {
                ProductFieldAdded = "Некорректно введена цена!";
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
            await ShopClient!.AddProduct(newProduct, _cts.Token);
            ProductFieldAdded = "Товар добавлен!";
            await InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}