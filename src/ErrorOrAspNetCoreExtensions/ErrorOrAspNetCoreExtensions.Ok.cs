using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions;

/// <summary>
/// A set of ErrorOr extension methods for ASP.NET Core
/// </summary>
public static partial class ErrorOrAspNetCoreExtensions
{
    /// <summary>
    /// Creates either <see cref="Ok{TResult}"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <returns>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToOk<TResult>(this ErrorOr<TResult> result) =>
        result.Match(TypedResults.Ok, ToProblem);

    /// <summary>
    /// Creates either <see cref="Ok{TValue}"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="mapper">A mapper function that converts successful result
    /// to the API response contract.</param>
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <typeparam name="TContract">Type of the response contract.</typeparam>
    /// <returns>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToOk<TResult, TContract>(
        this ErrorOr<TResult> result,
        Func<TResult, TContract> mapper
    ) => result.Match(value => TypedResults.Ok(mapper(value)), ToProblem);
}
