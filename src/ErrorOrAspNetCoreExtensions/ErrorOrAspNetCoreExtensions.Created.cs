using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace ErrorOrAspNetCoreExtensions;

public static partial class ErrorOrAspNetCoreExtensions
{
    public static IResult ToCreatedWithoutBody(this IErrorOr result, Uri createdAtUri) =>
        result.IsError ? result.Errors!.ToProblem() : TypedResults.Created(createdAtUri);

    public static IResult ToCreatedWithoutBody(this IErrorOr result, string createdAtUri) =>
        result.IsError ? result.Errors!.ToProblem() : TypedResults.Created(createdAtUri);

    public static IResult ToCreated<TResult>(this ErrorOr<TResult> result, Uri createdAtUri) =>
        result.Match(value => TypedResults.Created(createdAtUri, value), ToProblem);

    public static IResult ToCreated<TResult>(this ErrorOr<TResult> result, string createdAtUri) =>
        result.Match(value => TypedResults.Created(createdAtUri, value), ToProblem);

    public static IResult ToCreated<TResult, TContract>(
        this ErrorOr<TResult> result,
        Uri createdAtUri,
        Func<TResult, TContract> mapper
    ) => result.Match(value => TypedResults.Created(createdAtUri, mapper(value)), ToProblem);

    public static IResult ToCreated<TResult, TContract>(
        this ErrorOr<TResult> result,
        string createdAtUri,
        Func<TResult, TContract> mapper
    ) => result.Match(value => TypedResults.Created(createdAtUri, mapper(value)), ToProblem);
}
