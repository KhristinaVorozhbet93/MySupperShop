using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpApiClient.Exceptions;
using OnlineShop.HttpModels.Requests;

namespace OnlineShop.Frontend.Pages
{
    public partial class LoginPage : IDisposable
    {
        LoginRequest model = new();
        [Inject]
        public NavigationManager Manager { get; set; }
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }
        private CancellationTokenSource _cts = new CancellationTokenSource();
        [Inject]
        private IDialogService DialogService { get; set; }
        private bool _loginInProgress;
        bool isShow;

        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        public void Dispose()
        {
            _cts.Cancel();
        }
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
                await ShopClient!.Login(model, _cts.Token);
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