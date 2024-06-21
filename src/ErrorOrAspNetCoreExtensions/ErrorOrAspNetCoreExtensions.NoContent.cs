using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions;

public static partial class ErrorOrAspNetCoreExtensions
{
    /// <summary>
    /// Creates either <see cref="NoContent"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToNoContent(this IErrorOr result) =>
        result.IsError ? result.Errors!.ToProblem() : TypedResults.NoContent();
}
