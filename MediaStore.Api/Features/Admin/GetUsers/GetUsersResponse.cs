namespace MediaStore.Api.Features.Admin.GetUsers;

public record GetUsersResponse(
    Guid Id,
    string Email,
    string Role,
    string Status);