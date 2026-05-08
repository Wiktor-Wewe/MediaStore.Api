using FluentValidation;
using MediaStore.Api.Features.Products.CreateProduct;
using MediaStore.Api.Features.Products.GetProducts;
using MediaStore.Api.Infrastructure;
using MediaStore.Api.Infrastructure.Configuration;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//builder.Services.AddControllers(); // only minimal api
builder.Services.AddOpenApi();

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
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Product Catalog API")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
//app.UseAuthorization(); // TODO

app.UseCors();

//app.MapControllers(); // only minimal api
app.MapCreateProduct();
app.MapGetProducts();

app.Run();
