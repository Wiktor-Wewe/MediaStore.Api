using MediaStore.Api.Domain;
using MediaStore.Api.Domain.Errors;
using System.Collections.Concurrent;

namespace MediaStore.Api.Infrastructure.Persistence;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();
    private readonly ConcurrentDictionary<string, Guid> _codeIndex = new(StringComparer.OrdinalIgnoreCase);

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken ct)
        => Task.FromResult<IReadOnlyCollection<Product>>(_products.Values.ToList().AsReadOnly());

    public Task<Result> AddAsync(Product product, CancellationToken ct)
    {
        // Thread-Safety
        if (!_codeIndex.TryAdd(product.Code, product.Id))
        {
            return Task.FromResult(Result.Failure(ProductErrors.CodeAlreadyExists));
        }

        if (!_products.TryAdd(product.Id, product))
        {
            _codeIndex.TryRemove(product.Code, out _);
            return Task.FromResult(Result.Failure(ProductErrors.CreateFailed));
        }

        return Task.FromResult(Result.Success());
    }
}