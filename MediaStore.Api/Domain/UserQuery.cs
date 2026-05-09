namespace MediaStore.Api.Domain;

public record UserQuery(
    string? Email,
    UserStatus? Status,
    string SortBy,
    string SortDirection,
    int PageNumber,
    int PageSize);