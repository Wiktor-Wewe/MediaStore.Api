using MediaStore.Api.Domain;
using System.Collections.Concurrent;

namespace MediaStore.Api.Infrastructure.Persistence;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();
    private readonly ConcurrentDictionary<string, Guid> _codeIndex = new(StringComparer.OrdinalIgnoreCase);

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken ct)
        => Task.FromResult<IReadOnlyCollection<Product>>(_products.Values.ToList().AsReadOnly());

    public async Task<Result> AddAsync(Product product, CancellationToken ct)
    {
        // Thread-Safety
        if (!_codeIndex.TryAdd(product.Code, product.Id))
        {
            return Result.Failure("Error.Product.CodeAlreadyExists");
        }

        _products.TryAdd(product.Id, product);

        return Result.Success();
    }
}