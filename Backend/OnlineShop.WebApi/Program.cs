using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.EntityFramework.Data;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;

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
builder.Services.AddScoped<AccountService>();

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

app.MapControllers();
app.Run();
