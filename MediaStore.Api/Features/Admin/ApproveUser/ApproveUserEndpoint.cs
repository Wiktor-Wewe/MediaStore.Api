using MediaStore.Api.Common;
using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Admin.ApproveUser;


public static class ApproveUserEndpoint
{
    public static void MapApproveUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/users/{id:guid}/approve", async (
            Guid id,
            IAuthRepository repository,
            CancellationToken ct) =>
        {
            var result = await repository.ApproveAsync(id, ct);

            return result.IsSuccess
                ? Results.NoContent()
                : result.ToValidationProblem(statusCode: StatusCodes.Status404NotFound);
        })
        .RequireAuthorization("AdminOnly");
    }
}