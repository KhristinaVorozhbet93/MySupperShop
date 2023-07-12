using MySupperShop.Models;
using MySupperShop.Interfaces;
using Microsoft.AspNetCore.Components;

namespace MySupperShop.Pages
{
    public partial class CatalogPage
    {
        //[Inject]
        //private ICatalog Catalog { get; set; } = null!;

        [Inject]
        private IMyShopClient ShopClient { get; set; } = null!;

        private List<Product> _products = null!;

        protected override async Task OnInitializedAsync()
        {
            _products = await ShopClient.GetProducts();
        }
    }
}
