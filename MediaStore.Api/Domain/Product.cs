namespace MediaStore.Api.Domain;

public class Product(Guid id, string code, string name, decimal price)
{
    public Guid Id { get; } = id;
    public string Code { get; } = code;
    public string Name { get; } = name;
    public decimal Price { get; } = price;
}