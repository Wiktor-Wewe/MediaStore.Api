namespace MediaStore.Api.Infrastructure.Persistence.Seed;

public record SeedProduct(
    string Code,
    string Name,
    decimal Price,
    string? ImageUrl,
    Dictionary<string, string>? Descriptions);