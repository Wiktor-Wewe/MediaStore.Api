using FluentValidation;
using MediaStore.Api.Domain.Errors;

namespace MediaStore.Api.Features.Admin.GetUsers;

public class GetUsersValidator : AbstractValidator<GetUsersRequest>
{
    private static readonly string[] AllowedSortBy = ["email", "status"];
    private static readonly string[] AllowedSortDirections = ["asc", "desc"];

    public GetUsersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(PaginationErrors.InvalidPageNumber);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PaginationErrors.InvalidPageSize);

        RuleFor(x => x.SortBy)
            .Must(x => string.IsNullOrWhiteSpace(x) || AllowedSortBy.Contains(x.ToLowerInvariant()))
            .WithMessage(AuthErrors.InvalidSortBy);

        RuleFor(x => x.SortDirection)
            .Must(x => string.IsNullOrWhiteSpace(x) || AllowedSortDirections.Contains(x.ToLowerInvariant()))
            .WithMessage(AuthErrors.InvalidSortDirection);
    }
}