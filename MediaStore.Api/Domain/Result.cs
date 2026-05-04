namespace MediaStore.Api.Domain;

public record Result
{
    public bool IsSuccess { get; }
    public string? ErrorCode { get; }
    public IDictionary<string, string[]>? ValidationErrors { get; }

    protected Result(bool isSuccess, string? errorCode = null, IDictionary<string, string[]>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        ValidationErrors = validationErrors;
    }

    public static Result Success() => new(true);
    public static Result Failure(string errorCode) => new(false, errorCode);
    public static Result Invalid(IDictionary<string, string[]> errors) => new(false, null, errors);
}