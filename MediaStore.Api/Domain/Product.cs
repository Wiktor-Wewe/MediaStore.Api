namespace MediaStore.Api.Domain;

public record Product(Guid Id, string Code, string Name, decimal Price);
