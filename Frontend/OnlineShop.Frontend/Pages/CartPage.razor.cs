using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class CartPage
    {
        [Inject] private IMyShopClient ShopClient { get; set; }
        private CartResponse cartResponse;
        private CancellationTokenSource _cts = new();

        protected override async Task OnInitializedAsync()
        {
            var account = await ShopClient.GetAccount(_cts.Token);
            cartResponse = await ShopClient.GetCart(account.Id, _cts.Token);      
        }
    }
}