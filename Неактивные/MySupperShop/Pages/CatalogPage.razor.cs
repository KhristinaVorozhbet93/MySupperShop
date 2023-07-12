using MySupperShop.Models;

namespace MySupperShop.Pages 
{
    public partial class CatalogPage
    {
        private List<Product> _products;

        protected override void OnInitialized()
        {
            _products = catalog.GetProducts();
        }
    }
}
