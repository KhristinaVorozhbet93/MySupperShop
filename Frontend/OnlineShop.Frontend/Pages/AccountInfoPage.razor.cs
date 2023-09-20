using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class AccountInfoPage
    {
        [Inject] private IMyShopClient ShopClient { get; set; }
        [Inject] public NavigationManager Manager { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private string state = string.Empty;
        private CancellationTokenSource _cts = new();
        private AccountRequest _accountRequest = new();
        private AccountResponse _accountResponse;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _accountResponse = await ShopClient.GetAccount(_cts.Token);
            _accountRequest.Login = _accountResponse.Login;
            _accountRequest.Email = _accountResponse.Email; 
            _accountRequest.Name = _accountResponse.Name;
            _accountRequest.LastName = _accountResponse.LastName;
            _accountRequest.Image = _accountResponse.Image; 
        }

        public async Task DeleteAccount()
        {
            try
            {
                bool? result = await DialogService.ShowMessageBox(
            "Информация",
            "Вы верены, что хотите удалить аккаунт?",
            yesText: "Да", cancelText: "Нет");
                state = result == null ? "No" : "Yes";
                if (state == "Yes")
                {
                    await ShopClient.DeleteAccount(_accountRequest, _cts.Token);
                    await DialogService.ShowMessageBox("Информация", "Аккаунт удален!");
                    await Task.Delay(TimeSpan.FromSeconds(2), _cts.Token);
                    Manager.NavigateTo("/");
                }
                if (state == "No")
                {
                    await DialogService.ShowMessageBox("Информация", "Аккаунт не удален!");
                    return;
                }
            }
            catch (ArgumentNullException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Аккаунт не удален!");
            }           
        }

        public async Task LogOut()
        {
            ShopClient.ResetAuthorizationToken();
            await ClearToken();
            await Task.Delay(TimeSpan.FromSeconds(2));
            Manager.NavigateTo("/");
        }

        public void ToAccountEditorPage()
        {
            Manager.NavigateTo("account/editor");
        }
    }
}