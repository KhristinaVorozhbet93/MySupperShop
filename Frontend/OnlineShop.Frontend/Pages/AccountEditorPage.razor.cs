using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.Frontend.Pages
{
    public partial class AccountEditorPage 
    {
        [Inject] private IMyShopClient ShopClient { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private CancellationTokenSource _cts = new();
        private AccountRequest _accountRequest = new();
        private AccountPasswordRequest _accountPasswordRequest = new();
        private AccountResponse _accountResponse;
        private string _confirmedPassword = string.Empty;
        private bool isShow;
        private InputType PasswordInput = InputType.Password;
        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _accountResponse = await ShopClient.GetAccount(_cts.Token);
            _accountRequest.Login = _accountResponse.Login;
            _accountRequest.Email = _accountResponse.Email;
            _accountRequest.Name = _accountResponse.Name;
            _accountRequest.LastName = _accountResponse.LastName;
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

        public async Task UpdateAccountData()
        {
            if (_accountRequest.Name == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введено имя!");
                return;
            }
            if (_accountRequest.LastName == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введена фамилия!");
                return;
            }
            if (_accountRequest.Email == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Некорректно введена почта!");
                return;
            }

            try
            {
                await ShopClient.UpdateAccountData(_accountRequest, _cts.Token);
                await DialogService.ShowMessageBox("Успешно", "Данные изменены!");
            }
            catch (InvalidOperationException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Данные не изменены!");
            }

        }

        public async Task UpdateAccountPassword()
        {
            if (_accountPasswordRequest.OldPassword == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Введите старый пароль!");
                return;
            }
            if (_accountPasswordRequest.NewPassword == string.Empty 
                || _confirmedPassword == string.Empty)
            {
                await DialogService.ShowMessageBox("Ошибка", "Введите новый пароль!");
                return;
            }

            if (_accountPasswordRequest.NewPassword != _confirmedPassword)
            {
                await DialogService.ShowMessageBox("Информация", "Новые пароли не сопадают!");
                return;
            }
            _accountPasswordRequest.Login = _accountResponse.Login;

            try
            {
                await ShopClient.UpdateAccountPassword(_accountPasswordRequest, _cts.Token);
                await DialogService.ShowMessageBox("Успешно", "Пароль успешно изменен!");
            }
            catch (InvalidOperationException)
            {
                await DialogService.ShowMessageBox("Ошибка", "Пароль не изменен!");
            }
        }
    }
}