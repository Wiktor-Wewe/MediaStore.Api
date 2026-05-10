using FluentValidation;
using MediaStore.Api.Domain.Errors;

namespace MediaStore.Api.Features.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    private static readonly string[] SupportedLanguages = ["pl", "en", "de", "cs"]; // todo move to appsettings

    public CreateProductValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(ProductErrors.CodeRequired)
            .MaximumLength(10)
            .WithMessage(ProductErrors.CodeMaxLength);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ProductErrors.NameRequired)
            .MaximumLength(100)
            .WithMessage(ProductErrors.NameMaxLength);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage(ProductErrors.PriceGreaterThanZero);

        RuleFor(x => x.ImageUrl)
            .Must(BeValidUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage(ProductErrors.ImageUrlInvalid);

        RuleFor(x => x.Descriptions)
            .Must(HaveOnlySupportedLanguages)
            .When(x => x.Descriptions is not null)
            .WithMessage(ProductErrors.DescriptionLanguageInvalid);

        RuleForEach(x => x.Descriptions!)
            .Must(x => string.IsNullOrWhiteSpace(x.Value) || x.Value.Length <= 1000)
            .When(x => x.Descriptions is not null)
            .WithMessage(ProductErrors.DescriptionTooLong);
    }

    private static bool BeValidUrl(string? url)
        => Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    private static bool HaveOnlySupportedLanguages(Dictionary<string, string>? descriptions)
        => descriptions is null || descriptions.Keys.All(language =>
            SupportedLanguages.Contains(language.ToLowerInvariant()));
}