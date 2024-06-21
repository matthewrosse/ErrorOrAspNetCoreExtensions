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

    [Fact]
    public void ToOk_ShouldReturnOkHttpResult_WhenResultIsSuccess()
    {
        // Arrange
        ErrorOr<Success> errorOrSuccess = new Success();

        // Act

        var result = errorOrSuccess.ToOk();

        // Assert

        result
            .Should()
            .BeOfType<Ok<Success>>()
            .Which.StatusCode.Should()
            .Be(StatusCodes.Status200OK);
    }

    [Fact]
    public void ToOk_ShouldReturnOkHttpResult_AndMapToContract_WhenResultIsSuccess()
    {
        // Arrange

        var testGuid = "4f048803-2f4b-444e-8570-95bb4d80d887";

        var id = Guid.Parse(testGuid);
        var firstName = "John";
        var lastName = "Smith";

        var expectedId = testGuid;
        var expectedName = $"{firstName} {lastName}";

        var expectedApiContractResponse = new UserApiResponse(expectedId, expectedName);

        var mapper = (User user) =>
            new UserApiResponse(user.Id.ToString(), $"{user.FirstName} {user.LastName}");

        ErrorOr<User> errorOr = new User(id, firstName, lastName);

        // Act

        var result = errorOr.ToOk(mapper);

        // Assert

        result
            .Should()
            .BeOfType<Ok<UserApiResponse>>()
            .Which.Should()
            .Match<Ok<UserApiResponse>>(
                r =>
                    r.StatusCode == StatusCodes.Status200OK
                    && r.Value != null
                    && r.Value.Equals(expectedApiContractResponse),
                "because status code should be 200 OK, and the success value should be mapped to the response contract"
            );
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

    private record User(Guid Id, string FirstName, string LastName);

    private record UserApiResponse(string Id, string Name);
}
