using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MySupperShop;
using MudBlazor.Services;
using MySupperShop.Interfaces;
using MySupperShop.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddSingleton<ICatalog, Catalog>();
//builder.Services.AddSingleton<IMyShopClient>(new MyShopClient("https://localhost:7252"));

await builder.Build().RunAsync();
