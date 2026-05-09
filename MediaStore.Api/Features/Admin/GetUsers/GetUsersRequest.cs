using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Admin.GetUsers;

public record GetUsersRequest(
    string? Email,
    UserStatus? Status,
    string? SortBy,
    string? SortDirection,
    int PageNumber = 1,
    int PageSize = 10);