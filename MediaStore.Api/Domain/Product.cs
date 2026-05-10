namespace MediaStore.Api.Domain;

public class Product(
    Guid id, 
    string code, 
    string name, 
    decimal price, 
    string? imageUrl,
    IReadOnlyDictionary<string, string> descriptions)
{
    public Guid Id { get; } = id;
    public string Code { get; } = code;
    public string Name { get; } = name;
    public decimal Price { get; } = price;
    public string? ImageUrl { get; } = imageUrl;
    public IReadOnlyDictionary<string, string> Descriptions { get; } = descriptions;
}