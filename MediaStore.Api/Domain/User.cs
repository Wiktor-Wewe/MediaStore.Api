namespace MediaStore.Api.Domain;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Email { get; init; } = default!;
    public string PasswordHash { get; init; } = default!;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
}

public enum UserRole
{
    //for possible expansion to include a normal user/client
    Admin
}

public enum UserStatus
{
    Pending,
    Active
}