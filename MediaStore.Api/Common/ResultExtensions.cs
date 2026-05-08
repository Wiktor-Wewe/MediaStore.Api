using MediaStore.Api.Domain;

namespace MediaStore.Api.Common;

public static class ResultExtensions
{
    public static IResult ToValidationProblem(this Result result, string propertyName = "General", int statusCode = StatusCodes.Status409Conflict)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.ValidationProblem(
            errors: new Dictionary<string, string[]> { { propertyName, [result.ErrorCode!] } },
            statusCode: statusCode
        );
    }
}
