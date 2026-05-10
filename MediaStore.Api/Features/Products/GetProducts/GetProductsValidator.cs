using FluentValidation;
using MediaStore.Api.Domain.Errors;

namespace MediaStore.Api.Features.Products.GetProducts;

public class GetProductsValidator : AbstractValidator<GetProductsRequest>
{
    private static readonly string[] AllowedSortBy = ["code", "name", "price"];
    private static readonly string[] AllowedSortDirections = ["asc", "desc"];

    public GetProductsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(PaginationErrors.InvalidPageNumber);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PaginationErrors.InvalidPageSize);

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue)
            .WithMessage(ProductErrors.InvalidMinPrice);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPrice.HasValue)
            .WithMessage(ProductErrors.InvalidMaxPrice);

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage(ProductErrors.InvalidPriceRange);

        RuleFor(x => x.SortBy)
            .Must(x => string.IsNullOrWhiteSpace(x) || AllowedSortBy.Contains(x.ToLowerInvariant()))
            .WithMessage(PaginationErrors.InvalidSortBy);

        RuleFor(x => x.SortDirection)
            .Must(x => string.IsNullOrWhiteSpace(x) || AllowedSortDirections.Contains(x.ToLowerInvariant()))
            .WithMessage(PaginationErrors.InvalidSortDirection);
    }
}
