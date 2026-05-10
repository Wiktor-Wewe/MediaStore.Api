using System.Text.Json;
using MediaStore.Api.Domain;
using MediaStore.Api.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MediaStore.Api.Infrastructure.Persistence.Seed;

public class DatabaseInitializer(
    IProductRepository productRepository,
    IWebHostEnvironment environment,
    IOptions<DatabaseInitializerSettings> options,
    ILogger<DatabaseInitializer> logger)
{
    private readonly DatabaseInitializerSettings _settings = options.Value;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        if (!_settings.Enabled)
        {
            logger.LogInformation("Database initializer is disabled.");
            return;
        }

        var filePath = Path.Combine(environment.ContentRootPath, _settings.FilePath);

        if (!File.Exists(filePath))
        {
            logger.LogWarning("Seed file was not found. Path: {FilePath}", filePath);
            return;
        }

        var existingProducts = await productRepository.GetPagedAsync(
            new ProductQuery(
                Search: null,
                MinPrice: null,
                MaxPrice: null,
                SortBy: "name",
                SortDirection: "asc",
                PageNumber: 1,
                PageSize: 1),
            ct);

        if (existingProducts.TotalCount > 0)
        {
            logger.LogInformation("Database already contains products. Seed skipped.");
            return;
        }

        var json = await File.ReadAllTextAsync(filePath, ct);

        var seedProducts = 
            JsonSerializer.Deserialize<List<SeedProduct>>(json, _jsonSerializerOptions) ?? [];

        foreach (var seedProduct in seedProducts)
        {
            var descriptions = seedProduct.Descriptions?
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .ToDictionary(
                    x => x.Key.ToLowerInvariant(),
                    x => x.Value.Trim())
                ?? [];

            var product = new Product(
                Guid.NewGuid(),
                seedProduct.Code.Trim(),
                seedProduct.Name.Trim(),
                seedProduct.Price,
                string.IsNullOrWhiteSpace(seedProduct.ImageUrl) ? null : seedProduct.ImageUrl.Trim(),
                descriptions);

            var result = await productRepository.AddAsync(product, ct);

            if (!result.IsSuccess)
            {
                logger.LogWarning(
                    "Could not seed product {ProductCode}. Error: {ErrorCode}",
                    seedProduct.Code,
                    result.ErrorCode);
            }
        }

        logger.LogInformation("Database seeded with {Count} products.", seedProducts.Count);
    }
}