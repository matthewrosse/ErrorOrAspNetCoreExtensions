using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace ErrorOrAspNetCoreExtensions;

public static partial class ErrorOrAspNetCoreExtensions
{
    public static IResult ToOk<TResult>(this ErrorOr<TResult> result) =>
        result.Match(TypedResults.Ok, ToProblem);

    public static IResult ToOk<TResult, TContract>(
        this ErrorOr<TResult> result,
        Func<TResult, TContract> mapper
    ) => result.Match(value => TypedResults.Ok(mapper(value)), ToProblem);
}
