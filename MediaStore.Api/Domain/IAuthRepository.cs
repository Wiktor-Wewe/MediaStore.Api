namespace MediaStore.Api.Domain;

public interface IAuthRepository
{
    Task<PagedResult<User>> GetUsersAsync(UserQuery query, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> AddAsync(User user, CancellationToken ct = default);
    Task<Result> ApproveAsync(Guid userId, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid userId, Guid currentUserId, CancellationToken ct = default);

    Task<bool> IsRegistrationEnabledAsync(CancellationToken ct = default);
    Task SetRegistrationEnabledAsync(bool enabled, CancellationToken ct = default);
}