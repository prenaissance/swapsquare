using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SwapSquare.Authentication.Application.Configuration;
using SwapSquare.Authentication.DataAccess;
using SwapSquare.Authentication.DataAccess.Persistance;
using SwapSquare.Authentication.Application;
using SwapSquare.Authentication.Api.Routes;
using SwapSquare.Common.DI;
using SwapSquare.Common.Settings;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// bind config for JwtSettings
builder.Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
builder.Services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));

JwtSettings jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()!;
RSA rsa = RSA.Create();
rsa.ImportFromPem(jwtSettings.PublicKey);
SecurityKey securityKey = new RsaSecurityKey(rsa);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.Authority = "swap-square";
        options.Audience = "swap-square";
        options.RequireHttpsMetadata = false;

        using var rsa = RSA.Create();
        rsa.ImportFromPem(jwtSettings.PublicKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "swap-square",
            ValidAudience = "swap-square",
            IssuerSigningKey = securityKey,
            RequireSignedTokens = true,
            RequireExpirationTime = true, // <- JWTs are required to have "exp" property set
            ValidateLifetime = true, // <- the "exp" will be validated
            ValidateAudience = true,
            ValidateIssuer = true,
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false },
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
});
builder.Services.AddCommonWeb();
builder.Services.AddDataAccess(configuration);
builder.Services.AddApplication(configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
dbContext.Database.EnsureCreated();
dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var api = app.MapGroup("/api");

api.MapAuthenticationRoutes();
api.MapUserRoutes();

app.Run();