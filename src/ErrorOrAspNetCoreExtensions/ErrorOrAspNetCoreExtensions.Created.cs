using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions;

public static partial class ErrorOrAspNetCoreExtensions
{
    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <returns>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreatedWithoutBody(this IErrorOr result) =>
        result.IsError ? result.Errors!.ToProblem() : TypedResults.Created();

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="uriFactory">A delegate that constructs the created at URI.</param>
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreatedWithoutBody<TResult>(
        this ErrorOr<TResult> result,
        Func<TResult, Uri> uriFactory
    ) => result.Match(successValue => TypedResults.Created(uriFactory(successValue)), ToProblem);

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="uriFactory">A delegate that constructs the created at URI.</param>
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreatedWithoutBody<TResult>(
        this ErrorOr<TResult> result,
        Func<TResult, string> uriFactory
    ) => result.Match(successValue => TypedResults.Created(uriFactory(successValue)), ToProblem);

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="uriFactory">A delegate that constructs the created at URI.</param>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult>(
        this ErrorOr<TResult> result,
        Func<TResult, Uri> uriFactory
    ) =>
        result.Match(
            successValue => TypedResults.Created(uriFactory(successValue), successValue),
            ToProblem
        );

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="uriFactory">A delegate that constructs the created at URI.</param>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult>(
        this ErrorOr<TResult> result,
        Func<TResult, string> uriFactory
    ) =>
        result.Match(
            successValue => TypedResults.Created(uriFactory(successValue), successValue),
            ToProblem
        );

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="uriFactory">A delegate that constructs the created at URI.</param>
    /// <param name="mapper">A mapper function that converts successful result
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <typeparam name="TContract">Type of the response contract.</typeparam>
    /// to the API response contract.</param>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult, TContract>(
        this ErrorOr<TResult> result,
        Func<TResult, Uri> uriFactory,
        Func<TResult, TContract> mapper
    ) =>
        result.Match(
            successValue => TypedResults.Created(uriFactory(successValue), mapper(successValue)),
            ToProblem
        );

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="uriFactory">A delegate that constructs the created at URI.</param>
    /// <param name="mapper">A mapper function that converts successful result
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <typeparam name="TContract">Type of the response contract.</typeparam>
    /// to the API response contract.</param>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult, TContract>(
        this ErrorOr<TResult> result,
        Func<TResult, string> uriFactory,
        Func<TResult, TContract> mapper
    ) =>
        result.Match(
            successValue => TypedResults.Created(uriFactory(successValue), mapper(successValue)),
            ToProblem
        );
}
