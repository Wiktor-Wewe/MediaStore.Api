namespace MediaStore.Api.Features.Products.CreateProduct;

public record CreateProductRequest(
    string Code, 
    string Name, 
    decimal Price,
    string? ImageUrl,
    Dictionary<string, string>? Descriptions);
