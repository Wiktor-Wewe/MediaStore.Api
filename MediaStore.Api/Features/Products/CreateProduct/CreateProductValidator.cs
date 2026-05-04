using FluentValidation;

namespace MediaStore.Api.Features.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("Error.Product.Code");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Error.Product.Name");

        RuleFor(x => x.Price)
            .NotEmpty()
            .Must(x => decimal.TryParse(x, out var val) && val > 0)
            .WithMessage("Error.Product.Price");
    }
}