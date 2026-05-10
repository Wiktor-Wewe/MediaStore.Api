using MediaStore.Api.Common;
using MediaStore.Api.Domain;
using MediaStore.Api.Domain.Errors;
using MediaStore.Api.Infrastructure.Security;

namespace MediaStore.Api.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static void MapRegister(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (
            RegisterRequest request,
            IAuthRepository repository,
            PasswordHasher passwordHasher,
            CancellationToken ct) =>
        {
            var registrationEnabled = await repository.IsRegistrationEnabledAsync(ct);

            if (!registrationEnabled)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["General"] = [AuthErrors.RegistrationDisabled]
                });
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHasher.Hash(request.Password),
                Role = UserRole.Admin,
                Status = UserStatus.Pending
            };

            var result = await repository.AddAsync(user, ct);

            return result.IsSuccess
                ? Results.Accepted()
                : result.ToValidationProblem();
        })
        .AddEndpointFilter<ValidationFilter<RegisterRequest>>()
        .WithTags("Auth");
    }
}
