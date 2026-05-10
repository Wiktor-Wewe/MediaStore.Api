namespace MediaStore.Api.Domain;

public interface IProductRepository
{
    Task<PagedResult<Product>> GetPagedAsync(ProductQuery query, CancellationToken ct = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> AddAsync(Product product, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
