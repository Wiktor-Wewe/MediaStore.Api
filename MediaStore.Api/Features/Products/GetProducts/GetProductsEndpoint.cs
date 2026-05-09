using MediaStore.Api.Common;
using MediaStore.Api.Common.Pagination;
using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Products.GetProducts;

public static class GetProductsEndpoint
{
    public static void MapGetProducts(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (
            [AsParameters] GetProductsRequest request,
            IProductRepository repository, 
            CancellationToken ct) =>
        {
            var query = new ProductQuery(
                request.Search,
                request.MinPrice,
                request.MaxPrice,
                request.SortBy?.ToLowerInvariant() ?? "name",
                request.SortDirection?.ToLowerInvariant() ?? "asc",
                request.PageNumber,
                request.PageSize);

            var result = await repository.GetPagedAsync(query, ct);

            var items = result.Items
                .Select(p => new GetProductsResponse(p.Id, p.Code, p.Name, p.Price))
                .ToList();

            var totalPages = (int)Math.Ceiling(result.TotalCount / (double)request.PageSize);

            return Results.Ok(new PagedResponse<GetProductsResponse>(
                items,
                request.PageNumber,
                request.PageSize,
                result.TotalCount,
                totalPages));
        })
        .AddEndpointFilter<ValidationFilter<GetProductsRequest>>();
    }
}
