namespace MediaStore.Api.Domain;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyCollection<User>> GetPendingUsersAsync(CancellationToken ct = default);
    Task<Result> AddAsync(User user, CancellationToken ct = default);
    Task<Result> ApproveAsync(Guid userId, CancellationToken ct = default);

    Task<bool> IsRegistrationEnabledAsync(CancellationToken ct = default);
    Task SetRegistrationEnabledAsync(bool enabled, CancellationToken ct = default);
}