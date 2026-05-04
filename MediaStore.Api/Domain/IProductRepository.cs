namespace MediaStore.Api.Domain;

public interface IProductRepository
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken ct = default);
    Task<Result> AddAsync(Product product, CancellationToken ct = default);
}
