using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Admin.SetRegistrationEnabled;

public static class SetRegistrationEnabledEndpoint
{
    public static void MapSetRegistrationEnabled(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/settings/registration", async (
            SetRegistrationEnabledRequest request,
            IAuthRepository repository,
            CancellationToken ct) =>
        {
            await repository.SetRegistrationEnabledAsync(request.Enabled, ct);

            return Results.NoContent();
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Admin");
    }
}