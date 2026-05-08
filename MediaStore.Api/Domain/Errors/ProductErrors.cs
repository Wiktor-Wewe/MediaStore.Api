namespace MediaStore.Api.Domain.Errors;

public class ProductErrors
{
    public const string CreateFailed = "Error.Product.CreateFailed";
    public const string CodeRequired = "Error.Product.Code.Required";
    public const string CodeMaxLength = "Error.Product.Code.MaxLength";
    public const string CodeAlreadyExists = "Error.Product.Code.AlreadyExists";
    public const string NameRequired = "Error.Product.Name.Required";
    public const string NameMaxLength = "Error.Product.Name.MaxLength";
    public const string PriceGreaterThanZero = "Error.Product.Price.GreaterThanZero";
}
