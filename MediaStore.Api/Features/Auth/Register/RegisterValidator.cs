using FluentValidation;
using MediaStore.Api.Domain.Errors;

namespace MediaStore.Api.Features.Auth.Register;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(AuthErrors.EmailRequired)
            .EmailAddress()
            .WithMessage(AuthErrors.EmailInvalid);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(AuthErrors.PasswordRequired)
            .MinimumLength(8)
            .WithMessage(AuthErrors.PasswordTooShort);
    }
}