using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SwapSquare.ApiGateway.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json");

builder.Services.AddOcelot();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerForOcelot(builder.Configuration, options =>
{
    options.GenerateDocsDocsForGatewayItSelf(o =>
    {
        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
    });
});
JwtSettings jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

RSA rsa = RSA.Create();
rsa.ImportFromPem(jwtSettings.PublicKey);
SecurityKey securityKey = new RsaSecurityKey(rsa);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI(options =>
    {
        options.PathToSwaggerGenerator = "/swagger/docs";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();
