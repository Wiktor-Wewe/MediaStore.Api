using MediaStore.Api.Domain;
using MediaStore.Api.Domain.Errors;
using MediaStore.Api.Infrastructure.Security;

namespace MediaStore.Api.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapLogin(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            LoginRequest request,
            IAuthRepository repository,
            PasswordHasher passwordHasher,
            JwtTokenGenerator tokenGenerator,
            CancellationToken ct) =>
        {
            var user = await repository.GetByEmailAsync(request.Email, ct);

            if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["General"] = [AuthErrors.InvalidCredentials]
                });
            }

            if (user.Status != UserStatus.Active)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["General"] = [AuthErrors.UserNotActive]
                });
            }

            var token = tokenGenerator.Generate(user);

            return Results.Ok(new LoginResponse(token));
        })
        .WithTags("Auth");
        // todo add validators?
    }
}
