using MediaStore.Api.Common;
using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static void MapCreateProduct(this IEndpointRouteBuilder app)
    {
        // TODO add pagination
        app.MapPost("/api/products", async (CreateProductRequest req, IProductRepository repo, CancellationToken ct) =>
        {
            var price = req.Price;
            var product = new Product(Guid.NewGuid(), req.Code!, req.Name!, price);

            var result = await repo.AddAsync(product, ct);

            return result.IsSuccess
                ? Results.Created($"/api/products/{product.Id}", product)
                : result.ToValidationProblem(propertyName: nameof(product.Code));
        })
        .AddEndpointFilter<ValidationFilter<CreateProductRequest>>();
    }
}
