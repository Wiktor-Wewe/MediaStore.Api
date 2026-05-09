using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Admin.GetPendingUsers;

public static class GetPendingUsersEndpoint
{
    public static void MapGetPendingUsers(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/users/pending", async (
            IAuthRepository repository,
            CancellationToken ct) =>
        {
            var users = await repository.GetPendingUsersAsync(ct);

            var response = users.Select(x => new PendingUserResponse(
                x.Id,
                x.Email,
                x.Role.ToString(),
                x.Status.ToString()
            ));

            return Results.Ok(response);
        })
        .RequireAuthorization("AdminOnly");
    }
}