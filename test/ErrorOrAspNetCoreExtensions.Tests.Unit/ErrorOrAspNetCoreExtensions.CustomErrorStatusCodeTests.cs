using System.Net.Mime;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions.Tests.Unit;

public class CustomErrorStatusCodeTests
{
    [Theory]
    [MemberData(
        nameof(
            ToProblem_ShouldReturnProblemHttpResultWithAppropriateStatusCode_WhenCustomErrorHasAdditionalMetadata_Data
        )
    )]
    public void ToProblem_ShouldReturnProblemHttpResultWithAppropriateStatusCode_WhenCustomErrorHasAdditionalMetadata(
        Error error,
        int expectedStatusCode
    )
    {
        var problemDetails = error.ToProblem();

        problemDetails
            .Should()
            .NotBeNull()
            .And.Match<ProblemHttpResult>(r =>
                r.StatusCode == expectedStatusCode
                && r.ContentType == MediaTypeNames.Application.ProblemJson
            );
    }

    public static IEnumerable<object[]> ToProblem_ShouldReturnProblemHttpResultWithAppropriateStatusCode_WhenCustomErrorHasAdditionalMetadata_Data() =>
        new[]
        {
            new object[]
            {
                Error.Custom(
                    1001,
                    "Custom.Code",
                    "Custom.Description",
                    new Dictionary<string, object>
                    {
                        { ErrorOrAspNetCoreExtensions.StatusCodeKey, StatusCodes.Status404NotFound }
                    }
                ),
                StatusCodes.Status404NotFound
            },

            [
                Error.Custom(
                    1002,
                    "Custom.Code",
                    "Custom.Description",
                    new Dictionary<string, object>
                    {
                        {
                            ErrorOrAspNetCoreExtensions.StatusCodeKey,
                            StatusCodes.Status508LoopDetected
                        }
                    }
                ),
                StatusCodes.Status508LoopDetected
            ],

            [
                Error.Custom(
                    1003,
                    "Custom.Code",
                    "Custom.Description",
                    new Dictionary<string, object>
                    {
                        { ErrorOrAspNetCoreExtensions.StatusCodeKey, 599 }
                    }
                ),
                599
            ]
        };
}
