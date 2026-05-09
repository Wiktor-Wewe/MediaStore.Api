using MediaStore.Api.Common;
using MediaStore.Api.Common.Pagination;
using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Admin.GetUsers;

public static class GetUsersEndpoint
{
    public static void MapGetUsers(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/users", async (
            [AsParameters] GetUsersRequest request,
            IAuthRepository repository,
            CancellationToken ct) =>
        {
            var query = new UserQuery(
                request.Email,
                request.Status,
                request.SortBy?.ToLowerInvariant() ?? "email",
                request.SortDirection?.ToLowerInvariant() ?? "asc",
                request.PageNumber,
                request.PageSize);

            var result = await repository.GetUsersAsync(query, ct);

            var items = result.Items
                .Select(x => new GetUsersResponse(
                    x.Id,
                    x.Email,
                    x.Role.ToString(),
                    x.Status.ToString()))
                .ToList();

            var totalPages = (int)Math.Ceiling(result.TotalCount / (double)request.PageSize);

            return Results.Ok(new PagedResponse<GetUsersResponse>(
                items,
                request.PageNumber,
                request.PageSize,
                result.TotalCount,
                totalPages));
        })
        .RequireAuthorization("AdminOnly")
        .AddEndpointFilter<ValidationFilter<GetUsersRequest>>();
    }
}