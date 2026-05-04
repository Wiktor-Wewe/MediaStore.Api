namespace MediaStore.Api.Features.Products.GetProducts;

public record GetProductsResponse(Guid Id, string Code, string Name, decimal Price);