namespace MediaStore.Api.Features.Products.GetProducts;

public record GetProductsRequest(
    string? Search,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy,
    string? SortDirection,
    int PageNumber = 1,
    int PageSize = 10);
