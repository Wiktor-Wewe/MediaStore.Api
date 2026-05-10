namespace MediaStore.Api.Domain.Errors;

public static class AuthErrors
{
    public const string EmailRequired = "Error.Auth.Email.Required";
    public const string EmailInvalid = "Error.Auth.Email.Invalid";
    public const string EmailAlreadyExists = "Error.Auth.Email.AlreadyExists";

    public const string PasswordRequired = "Error.Auth.Password.Required";
    public const string PasswordTooShort = "Error.Auth.Password.TooShort";

    public const string InvalidCredentials = "Error.Auth.InvalidCredentials";
    public const string UserNotActive = "Error.Auth.User.NotActive";
    public const string UserNotFound = "Error.Auth.User.NotFound";

    public const string RegistrationDisabled = "Error.Auth.Registration.Disabled";
    public const string CreateFailed = "Error.Auth.CreateFailed";

    public const string CannotDeleteSelf = "Error.Auth.User.CannotDeleteSelf";
    public const string CannotDeleteLastActiveAdmin = "Error.Auth.User.CannotDeleteLastActiveAdmin";
}