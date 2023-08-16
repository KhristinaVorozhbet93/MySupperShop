using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpApiClient.Exceptions;
using OnlineShop.HttpModels.Requests;

namespace OnlineShop.Frontend.Pages
{
    public partial class RegistrationPage : IDisposable
    {
        RegisterRequest model = new();
        [Inject]
        public NavigationManager Manager { get; set; }
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }
        private CancellationTokenSource _cts = new CancellationTokenSource();
        [Inject]
        private IDialogService DialogService { get; set; }
        private bool _registrationInProgress;
        public void Dispose()
        {
            _cts.Cancel();
        }
        private async Task ProcessRegistration()
        {
            if (_registrationInProgress)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Пожалуйста, подождите...", Severity.Info);
                return;
            }
            _registrationInProgress = true;
            try
            {
                var response = await ShopClient!.Register(model, _cts.Token);
                await DialogService.ShowMessageBox
                    ("Успешно", $"Поздравляем {response.Login}!Вы успешно зарегистрированы!");
                Manager.NavigateTo("/account/login");
                
            }
            catch (MyShopAPIException e)
            {
                _registrationInProgress = false;
                await DialogService.ShowMessageBox("Ошибка", e.Message);
            }
            finally
            {
                _registrationInProgress = false;
            }
        }
    }
}