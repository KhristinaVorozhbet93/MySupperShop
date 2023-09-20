using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpApiClient.Exceptions;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class LoginByPasswordPage 
    {      
        [Inject] public NavigationManager Manager { get; set; }
        [Inject] public IMyShopClient? ShopClient { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }     
        [Inject] private IDialogService DialogService { get; set; }
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private LoginRequest _loginRequest = new();
        private LoginResponse _loginResponse;
        private LoginByCodeRequest _loginByCodeRequest = new();
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
        private async Task ProcessLoginByPassword()
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
                _loginResponse = await ShopClient!.LoginByPassword(_loginRequest, _cts.Token);
                _loginByCodeRequest.CodeId = _loginResponse.ConfirmationCodeId;
                _loginByCodeRequest.Login = _loginResponse.Login;
                await DialogService.ShowMessageBox("Информация", "Теперь введите код в окне ниже");
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

        private async Task ProcessLoginByCode()
        {
            try
            {
                var response = await ShopClient!.LoginByCode(_loginByCodeRequest, _cts.Token);
                await DialogService.ShowMessageBox("Успех", $"Добро пожаловать {response.Login}");
                await LocalStorage.SetItemAsync("token", response.Token);
                Manager.NavigateTo("/catalog");
            }
            catch (MyShopAPIException e)
            {
                _loginInProgress = false;
                await DialogService.ShowMessageBox("Ошибка", e.Message);
            }
        }
    }
}