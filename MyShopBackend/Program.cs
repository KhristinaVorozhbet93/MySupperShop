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

app.MapPost("/add_product", AddProduct);
app.MapGet("/get_products", GetAllProducts);
app.MapGet("/get_product", GetProductById);
app.MapPost("/update_product", UpdateProduct);
app.MapPost("/delete_product", DeleteProduct);

async Task AddProduct(
    [FromBody] Product product, 
    IProductRepozitory productRepozitory,
    CancellationToken cancellationToken)
{
    await productRepozitory.AddProduct(product, cancellationToken);
}
async Task<IResult> GetProductById(
    [FromQuery] Guid id, 
    IProductRepozitory productRepozitory, 
    CancellationToken cancellationToken)
{
    return await productRepozitory.GetProductById(id, cancellationToken);
}
async Task<List<Product>> GetAllProducts(IProductRepozitory productRepozitory,
    CancellationToken cancellationToken)
{
    return await productRepozitory.GetAllProducts(cancellationToken);
}
async Task<IResult> UpdateProduct(
    Product newProduct, 
    IProductRepozitory productRepozitory,
    CancellationToken cancellationToken)
{
    return await productRepozitory.UpdateProduct(newProduct, cancellationToken);
}
async Task DeleteProduct(
    [FromBody] Product product,
    IProductRepozitory productRepozitory,
    CancellationToken cancellationToken)
{
    await productRepozitory.DeleteProduct(product,cancellationToken);
}
app.MapControllers();
app.Run();
