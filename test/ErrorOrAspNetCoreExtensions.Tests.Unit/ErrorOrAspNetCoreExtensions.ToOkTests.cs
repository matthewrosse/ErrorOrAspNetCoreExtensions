using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions.Tests.Unit;

public class ToOkTests
{
    [Theory]
    [MemberData(nameof(ToOk_ShouldReturnProblemDetailsHttpResult_WhenResultIsError_Data))]
    public void ToOk_ShouldReturnProblemDetailsHttpResult_WhenResultIsError(
        ErrorOr<Success> errorOr,
        int expectedStatusCode
    )
    {
        // Arrange
        ErrorOr<Success> errorOrSuccess = errorOr;

        // Act

        var result = errorOrSuccess.ToOk();

        // Assert

        result
            .Should()
            .BeOfType<ProblemHttpResult>()
            .Which.StatusCode.Should()
            .Be(expectedStatusCode);
    }

    [Fact]
    public void ToOk_ShouldReturnValidationProblem_WhenResultIsError()
    {
        // Arrange
        ErrorOr<Success> errorOrSuccess = Error.Validation();

        // Act

        var result = errorOrSuccess.ToOk();

        // Assert

        result
            .Should()
            .BeOfType<ValidationProblem>()
            .Which.StatusCode.Should()
            .Be(StatusCodes.Status400BadRequest);
    }

    public static IEnumerable<object[]> ToOk_ShouldReturnProblemDetailsHttpResult_WhenResultIsError_Data() =>
        new[]
        {
            new object[] { Error.NotFound(), StatusCodes.Status404NotFound },
            [Error.Conflict(), StatusCodes.Status409Conflict],
            [Error.Forbidden(), StatusCodes.Status403Forbidden],
            [Error.Unauthorized(), StatusCodes.Status401Unauthorized],
            [Error.Failure(), StatusCodes.Status500InternalServerError],
        };
}
