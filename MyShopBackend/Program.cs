using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopBackend.Data;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string path = "myapp.db";

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlite($"DataSource = {path}"));
if (path == null) throw new Exception();

builder.Services.AddScoped<IProductRepozitory, ProductRepozitory>();
builder.Services.AddScoped<IAccountRepozitory, AccountRepozitory>();
builder.Services.AddScoped(typeof(IRepozitory<>), typeof(EfRepozitory<>));

var app = builder.Build();
app.UseCors(policy =>
{
    policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapPost("/add_account", AddAccount);
app.MapGet("/get_account_by_id", GetAccountById);
app.MapGet("/get_account_by_email", GetAccountByEmail);
//app.MapPost("/add_product", AddProduct);
//app.MapGet("/get_products", GetAllProducts);
//app.MapGet("/get_product", GetProductById);
//app.MapPost("/update_product", UpdateProduct);
//app.MapPost("/delete_product", DeleteProduct);

async Task AddAccount(
    [FromBody] Account account,
    IRepozitory<Account> accountRepozitory,
    CancellationToken cancellationToken)
{
    await accountRepozitory.Add(account, cancellationToken);
}
async Task<Account> GetAccountById(
    [FromQuery] Guid id,
    IRepozitory<Account> accountRepozitory,
    CancellationToken cancellationToken)
{
    return await accountRepozitory.GetById(id, cancellationToken);
}
async Task<Account> GetAccountByEmail(
    [FromQuery] string email,
    IAccountRepozitory accountRepozitory,
    CancellationToken cancellationToken)
{
    return await accountRepozitory.GetByEmail(email, cancellationToken);
}


//async Task AddProduct(
//    [FromBody] Product product,
//    IRepozitory<Product> productRepozitory,
//    CancellationToken cancellationToken)
//{
//    await productRepozitory.Add(product, cancellationToken);
//}
//async Task<Product> GetProductById(
//    [FromQuery] Guid id,
//    IRepozitory<Product> productRepozitory,
//    CancellationToken cancellationToken)
//{
//    return await productRepozitory.GetById(id, cancellationToken);
//}
//async Task<List<Product>> GetAllProducts(IRepozitory<Product> productRepozitory,
//    CancellationToken cancellationToken)
//{
//    return await productRepozitory.GetAll(cancellationToken);
//}
//async Task UpdateProduct(
//    Product newProduct, 
//    IProductRepozitory productRepozitory,
//    CancellationToken cancellationToken)
//{
//   await productRepozitory.Update(newProduct, cancellationToken);
//}
//async Task DeleteProduct(
//    [FromBody] Product product,
//    IRepozitory<Product> productRepozitory,
//    CancellationToken cancellationToken)
//{
//    await productRepozitory.Delete(product,cancellationToken);
//}
app.MapControllers();
app.Run();
