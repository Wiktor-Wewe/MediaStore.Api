using MediaStore.Api.Domain;
using MediaStore.Api.Domain.Errors;
using MediaStore.Api.Infrastructure.Security;
using System.Collections.Concurrent;

namespace MediaStore.Api.Infrastructure.Persistence;

public class InMemoryAuthRepository : IAuthRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();
    private readonly ConcurrentDictionary<string, Guid> _emailIndex = new(StringComparer.OrdinalIgnoreCase);

    private bool _registrationEnabled = true;

    // only for the purpose of the task (in memory)
    public InMemoryAuthRepository(PasswordHasher passwordHasher)
    {
        var admin = new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Email = "admin@mediastore.local",
            PasswordHash = passwordHasher.Hash("Admin123!"),
            Role = UserRole.Admin,
            Status = UserStatus.Active
        };

        _users.TryAdd(admin.Id, admin);
        _emailIndex.TryAdd(admin.Email, admin.Id);
    }

    public Task<PagedResult<User>> GetUsersAsync(UserQuery query, CancellationToken ct = default)
    {
        var users = _users.Values.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Email))
        {
            var email = query.Email.Trim();

            users = users.Where(x =>
                x.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
        }

        if (query.Status.HasValue)
            users = users.Where(x => x.Status == query.Status.Value);

        users = query.SortBy switch
        {
            "email" => query.SortDirection == "desc"
                ? users.OrderByDescending(x => x.Email)
                : users.OrderBy(x => x.Email),

            "status" => query.SortDirection == "desc"
                ? users.OrderByDescending(x => x.Status)
                : users.OrderBy(x => x.Status),

            _ => query.SortDirection == "desc"
                ? users.OrderByDescending(x => x.Email)
                : users.OrderBy(x => x.Email)
        };

        var totalCount = users.Count();

        var items = users
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(new PagedResult<User>(items, totalCount));
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        if (!_emailIndex.TryGetValue(email, out var id))
            return Task.FromResult<User?>(null);

        _users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<Result> AddAsync(User user, CancellationToken ct = default)
    {
        if (!_emailIndex.TryAdd(user.Email, user.Id))
            return Task.FromResult(Result.Failure(AuthErrors.EmailAlreadyExists));

        if (!_users.TryAdd(user.Id, user))
        {
            _emailIndex.TryRemove(user.Email, out _);
            return Task.FromResult(Result.Failure(AuthErrors.CreateFailed));
        }

        return Task.FromResult(Result.Success());
    }

    public Task<Result> ApproveAsync(Guid userId, CancellationToken ct = default)
    {
        if (!_users.TryGetValue(userId, out var user))
            return Task.FromResult(Result.Failure(AuthErrors.UserNotFound));

        user.Status = UserStatus.Active;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(Guid userId, Guid currentUserId, CancellationToken ct = default)
    {
        if (!_users.TryGetValue(userId, out var user))
            return Task.FromResult(Result.Failure(AuthErrors.UserNotFound));

        var activeAdminsCount = _users.Values.Count(x =>
            x.Role == UserRole.Admin &&
            x.Status == UserStatus.Active);

        if (user.Role == UserRole.Admin &&
            user.Status == UserStatus.Active &&
            activeAdminsCount <= 1)
        {
            return Task.FromResult(Result.Failure(AuthErrors.CannotDeleteLastActiveAdmin));
        }

        if (userId == currentUserId)
            return Task.FromResult(Result.Failure(AuthErrors.CannotDeleteSelf));

        if (!_users.TryRemove(userId, out var removedUser))
            return Task.FromResult(Result.Failure(AuthErrors.UserNotFound));

        _emailIndex.TryRemove(removedUser.Email, out _);

        return Task.FromResult(Result.Success());
    }

    public Task<bool> IsRegistrationEnabledAsync(CancellationToken ct = default)
        => Task.FromResult(_registrationEnabled);

    public Task SetRegistrationEnabledAsync(bool enabled, CancellationToken ct = default)
    {
        _registrationEnabled = enabled;
        return Task.CompletedTask;
    }
}