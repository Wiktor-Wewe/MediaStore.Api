using FluentValidation;
using MediaStore.Api.Domain.Errors;

namespace MediaStore.Api.Features.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
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
    }
}