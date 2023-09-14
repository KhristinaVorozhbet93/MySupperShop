using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiClient;

namespace OnlineShop.Frontend.Pages
{
    public class AppComponentBase : ComponentBase
    {
        [Inject] protected IMyShopClient ShopClient { get; private set; }
        [Inject] protected ILocalStorageService LocalStorage { get; private set; }
        [Inject] protected AppState State { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (State.IsTokenChecked) return;
            State.IsTokenChecked = true;

            string? token = await LocalStorage.GetItemAsync<string>("token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                ShopClient.SetAuthorizationToken(token);
            }
        }
    }
}
