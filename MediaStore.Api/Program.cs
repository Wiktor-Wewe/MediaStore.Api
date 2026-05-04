using FluentValidation;
using MediaStore.Api.Features.Products.CreateProduct;
using MediaStore.Api.Features.Products.GetProducts;
using MediaStore.Api.Infrastructure;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// fix problem while someone try to send "" as decimal
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//builder.Services.AddControllers(); // only minimal api
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
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
//app.UseAuthorization();

app.UseCors();

//app.MapControllers(); // only minimal api
app.MapCreateProduct();
app.MapGetProducts();

app.Run();
