using MediaStore.Api.Domain;
using MediaStore.Api.Domain.Errors;
using System.Collections.Concurrent;

namespace MediaStore.Api.Infrastructure.Persistence;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();
    private readonly ConcurrentDictionary<string, Guid> _codeIndex = new(StringComparer.OrdinalIgnoreCase);

    public Task<PagedResult<Product>> GetPagedAsync(ProductQuery query, CancellationToken ct)
    {
        var products = _products.Values.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            products = products.Where(p =>
                p.Code.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (query.MinPrice.HasValue)
            products = products.Where(p => p.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            products = products.Where(p => p.Price <= query.MaxPrice.Value);

        products = query.SortBy switch
        {
            "code" => query.SortDirection == "desc"
                ? products.OrderByDescending(p => p.Code)
                : products.OrderBy(p => p.Code),

            "price" => query.SortDirection == "desc"
                ? products.OrderByDescending(p => p.Price)
                : products.OrderBy(p => p.Price),

            _ => query.SortDirection == "desc"
                ? products.OrderByDescending(p => p.Name)
                : products.OrderBy(p => p.Name)
        };

        var totalCount = products.Count();

        var items = products
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(new PagedResult<Product>(items, totalCount));
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _products.TryGetValue(id, out var product);

        return Task.FromResult(product);
    }

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

    public Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (!_products.TryRemove(id, out var removedProduct))
        {
            return Task.FromResult(Result.Failure(ProductErrors.NotFound));
        }

        _codeIndex.TryRemove(removedProduct.Code, out _);

        return Task.FromResult(Result.Success());
    }
}