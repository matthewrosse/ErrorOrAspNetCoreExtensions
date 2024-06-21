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
    /// <param name="createdAtUri">Created at uri.</param>
    /// <returns>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreatedWithoutBody(this IErrorOr result, Uri createdAtUri) =>
        result.IsError ? result.Errors!.ToProblem() : TypedResults.Created(createdAtUri);

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="createdAtUri">Created at URI.</param>
    /// <returns>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreatedWithoutBody(this IErrorOr result, string createdAtUri) =>
        result.IsError ? result.Errors!.ToProblem() : TypedResults.Created(createdAtUri);

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="createdAtUri">The URI that points to the newly created resource.</param>
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult>(this ErrorOr<TResult> result, Uri createdAtUri) =>
        result.Match(value => TypedResults.Created(createdAtUri, value), ToProblem);

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="createdAtUri">The URI that points to the newly created resource.</param>
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult>(this ErrorOr<TResult> result, string createdAtUri) =>
        result.Match(value => TypedResults.Created(createdAtUri, value), ToProblem);

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="createdAtUri">The URI that points to the newly created resource.</param>
    /// <param name="mapper">A mapper function that converts successful result
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <typeparam name="TContract">Type of the response contract.</typeparam>
    /// to the API response contract.</param>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult, TContract>(
        this ErrorOr<TResult> result,
        Uri createdAtUri,
        Func<TResult, TContract> mapper
    ) => result.Match(value => TypedResults.Created(createdAtUri, mapper(value)), ToProblem);

    /// <summary>
    /// Creates either <see cref="Microsoft.AspNetCore.Http.HttpResults.Created"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="createdAtUri">The URI that points to the newly created resource.</param>
    /// <param name="mapper">A mapper function that converts successful result
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <typeparam name="TContract">Type of the response contract.</typeparam>
    /// to the API response contract.</param>
    /// <returns>An instance of <see cref="IResult"/>An instance of <see cref="IResult"/>.</returns>
    public static IResult ToCreated<TResult, TContract>(
        this ErrorOr<TResult> result,
        string createdAtUri,
        Func<TResult, TContract> mapper
    ) => result.Match(value => TypedResults.Created(createdAtUri, mapper(value)), ToProblem);
}
