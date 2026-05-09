namespace MediaStore.Api.Domain.Errors;

public class ProductErrors
{
    // create product
    public const string CreateFailed = "Error.Product.CreateFailed";
    public const string CodeRequired = "Error.Product.Code.Required";
    public const string CodeMaxLength = "Error.Product.Code.MaxLength";
    public const string CodeAlreadyExists = "Error.Product.Code.AlreadyExists";
    public const string NameRequired = "Error.Product.Name.Required";
    public const string NameMaxLength = "Error.Product.Name.MaxLength";
    public const string PriceGreaterThanZero = "Error.Product.Price.GreaterThanZero";

    // get products
    public const string InvalidPageNumber = "Error.Pagination.PageNumber.Invalid"; // move to paginationErros.cs?
    public const string InvalidPageSize = "Error.Pagination.PageSize.Invalid"; // -||-
    public const string InvalidMinPrice = "Error.Product.MinPrice.Invalid";
    public const string InvalidMaxPrice = "Error.Product.MaxPrice.Invalid";
    public const string InvalidPriceRange = "Error.Product.PriceRange.Invalid";
    public const string InvalidSortBy = "Error.Product.SortBy.Invalid";
    public const string InvalidSortDirection = "Error.Product.SortDirection.Invalid";
}
