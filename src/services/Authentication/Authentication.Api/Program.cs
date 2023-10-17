using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SwapSquare.Authentication.Application.Configuration;
using SwapSquare.Authentication.DataAccess;
using SwapSquare.Authentication.DataAccess.Persistance;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// bind config for JwtSettings
builder.Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
JwtSettings jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "swap-square";
        options.Audience = "swap-square";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "swap-square",
            ValidateIssuer = true,
            ValidAudience = "swap-square",
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(jwtSettings.SymmetricKey)
            ),
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataAccess(configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
dbContext.Database.EnsureCreated();
dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();