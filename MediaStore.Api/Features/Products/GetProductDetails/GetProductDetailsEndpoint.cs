using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Products.GetProductDetails;

public static class GetProductDetailsEndpoint
{
    public static void MapGetProductDetails(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{id:guid}", async (
            Guid id,
            string? language,
            IProductRepository repository,
            CancellationToken ct) =>
        {
            var product = await repository.GetByIdAsync(id, ct);

            if (product is null)
            {
                return Results.NotFound();
            }

            var description = ResolveDescription(product.Descriptions, language);

            var response = new GetProductDetailsResponse(
                product.Id,
                product.Code,
                product.Name,
                product.Price,
                product.ImageUrl,
                description,
                product.Descriptions);

            return Results.Ok(response);
        })
        .WithTags("Products"); ;
    }

    private static string? ResolveDescription(
        IReadOnlyDictionary<string, string> descriptions,
        string? language)
    {
        if (descriptions.Count == 0)
        {
            return null;
        }

        var normalizedLanguage = string.IsNullOrWhiteSpace(language)
            ? "en"
            : language.ToLowerInvariant();

        if (descriptions.TryGetValue(normalizedLanguage, out var requestedDescription))
        {
            return requestedDescription;
        }

        if (descriptions.TryGetValue("en", out var englishDescription))
        {
            return englishDescription;
        }

        return descriptions.Values.FirstOrDefault();
    }
}