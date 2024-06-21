using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;

namespace ErrorOrAspNetCoreExtensions;

public static partial class ErrorOrAspNetCoreExtensions
{
    /// <summary>
    /// Creates either <see cref="FileStreamHttpResult"/>, <see cref="ProblemHttpResult"/>
    /// or a <see cref="ValidationProblem"/> from the <see cref="ErrorOr{TValue}"/> object.
    /// </summary>
    /// <param name="result">The <see cref="ErrorOr{TValue}"/> object.</param>
    /// <param name="enableRangeProcessing">A flag that determines if range processing is enabled.</param>
    /// <param name="entityTag">The <see cref="EntityTagHeaderValue"/> to be configure the ETag response header and perform conditional requests.</param>
    /// <typeparam name="TResult">Type of the success value.</typeparam>
    /// <returns></returns>
    public static IResult ToFileStream<TResult>(
        this ErrorOr<TResult> result,
        bool enableRangeProcessing = false,
        EntityTagHeaderValue? entityTag = null
    )
        where TResult : IFileStreamResult =>
        result.Match(
            value =>
                TypedResults.File(
                    value.FileContent,
                    value.ContentType,
                    value.DownloadFileName,
                    value.LastModified,
                    entityTag,
                    enableRangeProcessing
                ),
            ToProblem
        );
}
