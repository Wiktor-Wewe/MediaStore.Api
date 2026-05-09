namespace MediaStore.Api.Features.Admin.GetPendingUsers;

public record PendingUserResponse(
    Guid Id,
    string Email,
    string Role,
    string Status);