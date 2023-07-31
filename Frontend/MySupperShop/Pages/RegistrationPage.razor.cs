using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyShopBackend.Models;
using MySupperShopHttpApiClient.Data;
using MySupperShopHttpApiClient.Interfaces;

namespace MySupperShop.Pages
{
    public partial class RegistrationPage
    {
        RegisterRequest model = new ();
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }
        private CancellationTokenSource _cts = new CancellationTokenSource();
        [Inject] 
        private IDialogService DialogService { get; set; }
        private bool _registrationInProgress;
 
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
                await ShopClient!.Register(model, _cts.Token);
                await DialogService.ShowMessageBox("Успешно", "Вы успешно зарегистрированы!");
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