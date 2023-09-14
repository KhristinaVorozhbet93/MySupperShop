using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpApiClient.Exceptions;
using OnlineShop.HttpModels.Requests;

namespace OnlineShop.Frontend.Pages
{
    public partial class LoginPage 
    {
        
        [Inject] public NavigationManager Manager { get; set; }
        [Inject] public IMyShopClient? ShopClient { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }     
        [Inject] private IDialogService DialogService { get; set; }
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private LoginRequest model = new();
        private bool _loginInProgress;
        private bool isShow;

        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        void ShowPassword()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
        private async Task ProcessLogin()
        {
            if (_loginInProgress)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Пожалуйста, подождите...", Severity.Info);
                return;
            }
            _loginInProgress = true;
            try
            {
                var response = await ShopClient!.Login(model, _cts.Token);
                await DialogService.ShowMessageBox("Успешно",$"Добро пожаловать,{response.Name}");
                await LocalStorage.SetItemAsync("token", response.Token);
                Manager.NavigateTo("/catalog");
            }
            catch (MyShopAPIException e)
            {
                _loginInProgress = false;
                await DialogService.ShowMessageBox("Ошибка", e.Message);
            }
            finally
            {
                _loginInProgress = false;
            }
        }
    }
}