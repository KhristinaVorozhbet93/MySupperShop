using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Data.EntityFramework.Data;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.IdentityPasswordHasherLib;
using OnlineShop.MailKit.EmailSender;
using OnlineShop.WebApi.Configurations;
using OnlineShop.WebApi.Filtres;
using OnlineShop.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging
    (options => options.LoggingFields = HttpLoggingFields.All); 

JwtConfig jwtConfig = builder.Configuration
   .GetRequiredSection("JwtConfig")
   .Get<JwtConfig>();
if (jwtConfig is null)
{
    throw new InvalidOperationException("JwtConfig is not configured");
}
builder.Services.AddSingleton(jwtConfig);

builder.Services.AddOptions<SmtpConfig>()
    .BindConfiguration("SmtpConfig")
   .ValidateDataAnnotations()
   .ValidateOnStart();

string path = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlite($"DataSource = {path}"));
if (path == null) throw new Exception();

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CentralizedExceptionHandlingFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepozitory<>), typeof(EfRepozitory<>));
builder.Services.AddScoped<IProductRepozitory, ProductRepozitory>();
builder.Services.AddScoped<IAccountRepozitory, AccountRepozitory>();
builder.Services.AddScoped<ICartRepozitory, CartRepozitory>();
builder.Services.AddScoped<IConfirmationCodeRepozitory, ConfirmationCodeRepozitory>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEf>();
builder.Services.AddScoped<IEmailSender,MailKitSmptEmailSender>();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<CartService>();

builder.Services.AddSingleton<IApplicationPasswordHasher, IdentityPasswordHasher>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(AccountRegistredEvent).Assembly);
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,

            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudiences = new[] { jwtConfig.Audience },
            ValidIssuer = jwtConfig.Issuer
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(policy =>
{
    policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
});

app.UseHttpLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
