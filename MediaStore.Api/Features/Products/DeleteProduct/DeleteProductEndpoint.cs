using MediaStore.Api.Common;
using MediaStore.Api.Domain;
using MediaStore.Api.Domain.Errors;

namespace MediaStore.Api.Features.Products.DeleteProduct;

public static class DeleteProductEndpoint
{
    public static void MapDeleteProduct(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/products/{id:guid}", async (
            Guid id,
            IProductRepository repository,
            CancellationToken ct) =>
        {
            var result = await repository.DeleteAsync(id, ct);

            return result.IsSuccess
                ? Results.NoContent()
                : result.ToValidationProblem(
                    statusCode: result.ErrorCode == ProductErrors.NotFound
                        ? StatusCodes.Status404NotFound
                        : StatusCodes.Status409Conflict);
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Products");
    }
}