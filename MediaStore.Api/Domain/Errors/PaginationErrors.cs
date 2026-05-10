namespace MediaStore.Api.Domain.Errors;

public static class PaginationErrors
{
    public const string InvalidPageNumber = "Error.Pagination.PageNumber.Invalid";
    public const string InvalidPageSize = "Error.Pagination.PageSize.Invalid";
    public const string InvalidSortBy = "Error.Pagination.SortBy.Invalid";
    public const string InvalidSortDirection = "Error.Pagination.SortDirection.Invalid";
}
