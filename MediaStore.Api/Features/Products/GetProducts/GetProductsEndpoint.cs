using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Products.GetProducts;

public static class GetProductsEndpoint
{
    public static void MapGetProducts(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (IProductRepository repository, CancellationToken ct) =>
        {
            var products = await repository.GetAllAsync(ct);
            var response = products.Select(p => new GetProductsResponse(p.Id, p.Code, p.Name, p.Price));

            return Results.Ok(response);
        });
    }
}
