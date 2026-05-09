namespace MediaStore.Api.Domain;

public record ProductQuery(
    string? Search,
    decimal? MinPrice,
    decimal? MaxPrice,
    string SortBy,
    string SortDirection,
    int PageNumber,
    int PageSize);