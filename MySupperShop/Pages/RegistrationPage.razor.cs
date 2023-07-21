using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MyShopBackend.Models;
using MySupperShopHttpApiClient.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MySupperShop.Pages
{
    public partial class RegistrationPage
    {
        RegisterAccountForm model = new RegisterAccountForm();
        bool success;
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
       
        private CancellationTokenSource _cts = new CancellationTokenSource();
        [Inject] 
        private IDialogService DialogService { get; set; }

        public class RegisterAccountForm
        {
            [Required]
            [StringLength(30, ErrorMessage = "Логин минимум 6 символов", MinimumLength = 6)]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "Пароль минимум 8 символов.", MinimumLength = 8)]
            public string Password { get; set; }

            [Required]
            [Compare(nameof(Password))]
            public string Password2 { get; set; }
        }
        private async Task OnValidSubmit(EditContext context)
        {
            var account = new Account(model.Username, model.Password, model.Email)
            {
                Id = Guid.NewGuid()
            };

            if (!IsPassword(model.Password))
            {
                await DialogService.ShowMessageBox("Ошибка","Пароль задан некорректно!" +
                    "Он должен содержать как минимум 8 символов");
                return; 
            }

            await ShopClient!.Register(account, _cts.Token);
            await DialogService.ShowMessageBox("Успешно", "Вы успешно зарегистрированы!");
            success = true;
            StateHasChanged();          
        }
        private static bool IsPassword(string value)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return hasNumber.IsMatch(value) && hasUpperChar.IsMatch(value) && hasMinimum8Chars.IsMatch(value);
        }
    }
}