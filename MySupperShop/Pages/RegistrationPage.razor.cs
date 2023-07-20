using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyShopBackend.Models;
using MySupperShopHttpApiClient.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MySupperShop.Pages
{
    public partial class RegistrationPage
    {
   
        RegisterAccountForm model = new RegisterAccountForm();
        bool success;
        [Inject]
        public IMyShopClient? ShopClient { get; set; }
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public class RegisterAccountForm
        {

            [Required]
            [StringLength(8, ErrorMessage = "Длина логина не может быть больше 8 символов")]
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
            await ShopClient!.Register(account, _cts.Token);
            success = true;
            StateHasChanged();          
        }


    }
}