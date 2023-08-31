using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using OnlineShop.Frontend;
using OnlineShop.HttpApiClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddSingleton(new MyShopClient("https://localhost:7252")); 
builder.Services.AddSingleton<IMyShopClient>(new MyShopClient("https://localhost:7252"));
builder.Services.AddSingleton<AppState>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
