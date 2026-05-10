using MediaStore.Api.Common;
using MediaStore.Api.Domain;
using System.Security.Claims;

namespace MediaStore.Api.Features.Admin.DeleteUser;

public static class DeleteUserEndpoint
{
    public static void MapDeleteUser(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/admin/users/{id:guid}", async (
            Guid id,
            ClaimsPrincipal user,
            IAuthRepository repository,
            CancellationToken ct) =>
        {
            var currentUserIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue("sub");

            if (!Guid.TryParse(currentUserIdValue, out var currentUserId))
            {
                return Results.Unauthorized();
            }

            var result = await repository.DeleteAsync(id, currentUserId, ct);

            return result.IsSuccess
                ? Results.NoContent()
                : result.ToValidationProblem(
                    statusCode: result.ErrorCode == "Error.Auth.User.NotFound"
                        ? StatusCodes.Status404NotFound
                        : StatusCodes.Status409Conflict);
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Admin");
    }
}