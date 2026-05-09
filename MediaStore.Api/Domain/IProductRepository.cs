namespace MediaStore.Api.Domain;

public interface IProductRepository
{
    Task<PagedResult<Product>> GetPagedAsync(ProductQuery query, CancellationToken ct = default);
    Task<Result> AddAsync(Product product, CancellationToken ct = default);
}
