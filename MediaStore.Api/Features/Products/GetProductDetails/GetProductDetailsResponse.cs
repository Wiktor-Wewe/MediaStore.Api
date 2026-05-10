namespace MediaStore.Api.Features.Products.GetProductDetails;

public record GetProductDetailsResponse(
    Guid Id,
    string Code,
    string Name,
    decimal Price,
    string? ImageUrl,
    string? Description,
    IReadOnlyDictionary<string, string> Descriptions);