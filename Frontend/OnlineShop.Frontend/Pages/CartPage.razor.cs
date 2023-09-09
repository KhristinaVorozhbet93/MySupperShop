using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class CartPage : IDisposable
    {
        [Inject] private IMyShopClient ShopClient { get; set; }
        CartResponse cartResponse;

        private CancellationTokenSource _cts = new();

        public void Dispose()
        {
            _cts.Cancel();
        }

        protected override async Task OnInitializedAsync()
        {
            var account = await ShopClient.GetAccount(_cts.Token);
            var cart = await ShopClient.GetCart(account.Id, _cts.Token);

            cartResponse = cart; 

         
        }
    }
}