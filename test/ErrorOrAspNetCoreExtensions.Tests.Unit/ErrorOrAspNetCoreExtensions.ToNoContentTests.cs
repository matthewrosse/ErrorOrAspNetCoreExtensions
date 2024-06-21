using System.Net.Mime;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions.Tests.Unit;

public class ToNoContentTests
{
    [Fact]
    public void ToNoContent_Should_ReturnNoContentHttpResult_WhenResultStateIsSuccess()
    {
        ErrorOr<Success> errorOr = new Success();

        var result = errorOr.ToNoContent();

        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void ToNoContent_Should_ReturnValidationProblem_WhenResultStateIsError()
    {
        ErrorOr<Success> errorOr = Error.Validation();

        var result = errorOr.ToNoContent();

        result
            .Should()
            .BeOfType<ValidationProblem>()
            .Which.StatusCode.Should()
            .Be(StatusCodes.Status400BadRequest);
    }

    [Theory]
    [MemberData(
        nameof(ToNoContent_ShouldReturnProblemDetailsHttpResult_WhenResultStateIsError_Data)
    )]
    public void ToNoContent_ShouldReturnProblemDetailsHttpResult_WhenResultStateIsError(
        Error error,
        int expectedStatusCode
    )
    {
        ErrorOr<Success> errorOr = error;

        var result = errorOr.ToNoContent();

        result
            .Should()
            .BeOfType<ProblemHttpResult>()
            .And.Match<ProblemHttpResult>(r =>
                r.StatusCode == expectedStatusCode
                && r.ContentType == MediaTypeNames.Application.ProblemJson
            );
    }

    public static IEnumerable<object[]> ToNoContent_ShouldReturnProblemDetailsHttpResult_WhenResultStateIsError_Data()
    {
        return new[]
        {
            new object[] { Error.Forbidden(), StatusCodes.Status403Forbidden },
            [Error.Unauthorized(), StatusCodes.Status401Unauthorized],
            [Error.Conflict(), StatusCodes.Status409Conflict],
            [Error.NotFound(), StatusCodes.Status404NotFound],
        };
    }
}
