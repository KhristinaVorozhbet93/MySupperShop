using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopBackend.Data;
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

async Task AddProduct([FromBody]Product product, [FromServices]AppDbContext dbContext)
{
    await dbContext.Products.AddAsync(product);
    await dbContext.SaveChangesAsync();
}
async Task<IResult> GetProductById([FromQuery] Guid id,[FromServices] AppDbContext dbContext)
{
    var product = await dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);
    if (product is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);  
}
async Task<List<Product>> GetAllProducts(AppDbContext dbContext)
{
    return await dbContext.Products.ToListAsync();
}
async Task<IResult> UpdateProduct([FromQuery] Guid guid, [FromBody] Product newProduct,
    [FromServices] AppDbContext dbContext)
{
    var product = await dbContext.Products
        .FirstOrDefaultAsync(product => product.Id == guid);
    if (product is null)
    {
        return Results.NotFound();
    }
    product!.Name = newProduct.Name;
    product.Price = newProduct.Price;

    await dbContext.SaveChangesAsync();
    return Results.Ok();
}
async Task DeleteProduct([FromBody] Product product,[FromServices] AppDbContext dbContext)
{
    dbContext.Products.Remove(product);
    await dbContext.SaveChangesAsync();
}
app.MapControllers();

app.Run();
