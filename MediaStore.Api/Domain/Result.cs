namespace MediaStore.Api.Domain;

public sealed record Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorCode { get; }

    private Result(bool isSuccess, string? errorCode)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(string errorCode)
    {
        if (string.IsNullOrWhiteSpace(errorCode))
            throw new ArgumentException("Error code cannot be empty.", nameof(errorCode));

        return new(false, errorCode);
    }
}