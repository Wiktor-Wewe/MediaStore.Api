using MediaStore.Api.Domain;

namespace MediaStore.Api.Features.Admin.GetRegistrationSettings;

public static class GetRegistrationSettingsEndpoint
{
    public static void MapGetRegistrationSettings(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/settings/registration", async (
            IAuthRepository repository,
            CancellationToken ct) =>
        {
            var enabled = await repository.IsRegistrationEnabledAsync(ct);

            return Results.Ok(new GetRegistrationSettingsResponse(enabled));
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Admin");
    }
}