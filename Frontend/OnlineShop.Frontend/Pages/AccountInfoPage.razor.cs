using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Requests;

namespace OnlineShop.Frontend.Pages
{
    public partial class AccountInfoPage : IDisposable
    {
        [Inject]
        private IMyShopClient ShopClient { get; set; }
        [Inject]
        public NavigationManager Manager { get; set; }
        private CancellationTokenSource _cts = new();
        AccountRequest model = new();

        public void Dispose()
        {
            _cts.Cancel();
        }
        protected override async Task OnInitializedAsync()
        {
            var account = await ShopClient.GetAccount(_cts.Token);
            model.Login = account.Login;
            model.Email = account.Email;
            model.Name = account.Name;
            model.LastName = account.LastName;
        }

        public void ToAccountEditorPage()
        {
            Manager.NavigateTo("account/editor");
        }
    }
}