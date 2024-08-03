using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions;

public static partial class ErrorOrAspNetCoreExtensions
{
    internal static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return TypedResults.Problem();
        }

        return errors.All(error => error.Type is ErrorType.Validation)
            ? errors.ToValidationProblem()
            : errors.First().ToProblem();
    }

    internal static ProblemHttpResult ToProblem(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => RetrieveStatusCodeFromNumericErrorTypeOrDefault(error)
        };

        return TypedResults.Problem(statusCode: statusCode, title: error.Code, detail: error.Description);

        int RetrieveStatusCodeFromNumericErrorTypeOrDefault(Error err)
        {
            if (err.Metadata is null)
            {
                return StatusCodes.Status500InternalServerError;
            }

            var value = err.Metadata.GetValueOrDefault(StatusCodeKey);

            return value is int intVal and >= 400 and < 600
                ? intVal
                : StatusCodes.Status500InternalServerError;
        }
    }

    private static ValidationProblem ToValidationProblem(this List<Error> errors)
    {
        var errorsDict = errors
            .GroupBy(e => e.Code)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

        return TypedResults.ValidationProblem(errorsDict);
    }
}
