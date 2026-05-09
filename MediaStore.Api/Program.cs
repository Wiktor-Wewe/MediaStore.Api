using FluentValidation;
using MediaStore.Api.Domain;
using MediaStore.Api.Features.Admin.ApproveUser;
using MediaStore.Api.Features.Admin.GetPendingUsers;
using MediaStore.Api.Features.Admin.SetRegistrationEnabled;
using MediaStore.Api.Features.Auth.Login;
using MediaStore.Api.Features.Auth.Register;
using MediaStore.Api.Features.Products.CreateProduct;
using MediaStore.Api.Features.Products.GetProducts;
using MediaStore.Api.Infrastructure;
using MediaStore.Api.Infrastructure.Configuration;
using MediaStore.Api.Infrastructure.OpenApi;
using MediaStore.Api.Infrastructure.Persistence;
using MediaStore.Api.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration); // select and register inmemory repos or normal base on appsettings
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddSingleton<PasswordHasher>();
// scalar
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// JWT
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

// CORS
builder.Services.Configure<CorsSettings>(
    builder.Configuration.GetSection("Cors"));

var corsSettings = builder.Configuration
    .GetSection("Cors")
    .Get<CorsSettings>() ?? new CorsSettings();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsSettings.AllowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // scalar
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Product Catalog API")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// redirection
app.UseHttpsRedirection();

// cors
app.UseCors();

// auth
app.UseAuthentication();
app.UseAuthorization();

// endpoints
app.MapLogin();
app.MapRegister();

app.MapGetPendingUsers();
app.MapApproveUser();
app.MapSetRegistrationEnabled();

app.MapCreateProduct();
app.MapGetProducts();

// run :D
app.Run();
