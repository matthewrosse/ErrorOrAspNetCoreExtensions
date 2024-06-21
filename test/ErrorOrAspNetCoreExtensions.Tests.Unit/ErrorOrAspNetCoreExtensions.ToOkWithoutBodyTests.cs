using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions.Tests.Unit;

public class ToOkWithoutBodyTests
{
    [Fact]
    public void ToOkWithoutBody_ShouldReturnOkHttpResult_WhenResultStateIsSuccess()
    {
        ErrorOr<Success> errorOr = new Success();

        var result = errorOr.ToOkWithoutBody();

        result.Should().BeOfType<Ok>();
    }

    [Theory]
    [MemberData(nameof(ToOkWithoutBody_ShouldReturnProblemHttpResult_WhenResultStateIsError_Data))]
    public void ToOkWithoutBody_ShouldReturnProblemHttpResult_WhenResultStateIsError(
        Error expectedError,
        int expectedStatusCode
    )
    {
        ErrorOr<Success> errorOr = expectedError;

        var result = errorOr.ToOkWithoutBody();

        result
            .Should()
            .BeOfType<ProblemHttpResult>()
            .And.Match<ProblemHttpResult>(r => r.StatusCode == expectedStatusCode);
    }

    [Fact]
    public void ToOkWithoutBody_ShouldReturnValidationProblem_WhenResultStateIsError()
    {
        ErrorOr<Success> errorOr = Error.Validation();

        var result = errorOr.ToOkWithoutBody();

        result
            .Should()
            .BeOfType<ValidationProblem>()
            .And.Match<ValidationProblem>(r => r.StatusCode == StatusCodes.Status400BadRequest);
    }

    public static IEnumerable<object[]> ToOkWithoutBody_ShouldReturnProblemHttpResult_WhenResultStateIsError_Data()
    {
        return new[]
        {
            new object[] { Error.NotFound(), StatusCodes.Status404NotFound, },
            [Error.Forbidden(), StatusCodes.Status403Forbidden],
            [Error.Unauthorized(), StatusCodes.Status401Unauthorized],
            [Error.Conflict(), StatusCodes.Status409Conflict],
        };
    }
}
