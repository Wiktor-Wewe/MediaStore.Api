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
    public const string ImageUrlInvalid = "Error.Product.ImageUrl.Invalid";
    public const string DescriptionLanguageInvalid = "Error.Product.Description.Language.Invalid";
    public const string DescriptionTooLong = "Error.Product.Description.TooLong";

    // get products
    public const string InvalidMinPrice = "Error.Product.MinPrice.Invalid";
    public const string InvalidMaxPrice = "Error.Product.MaxPrice.Invalid";
    public const string InvalidPriceRange = "Error.Product.PriceRange.Invalid";
}
