using System.Net.Mime;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorOrAspNetCoreExtensions.Tests.Unit;

public class ToCreatedTests
{
    [Theory]
    [MemberData(nameof(ToCreated_Uri_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data))]
    public void ToCreated_Uri_ShouldReturnCreatedHttpResult_WhenStateIsSuccess(
        Uri createdAtUri,
        TestResultObject resultObject
    )
    {
        ErrorOr<TestResultObject> errorOr = resultObject;

        var result = errorOr.ToCreated(createdAtUri);

        result
            .Should()
            .BeOfType<Created<TestResultObject>>()
            .And.Match<Created<TestResultObject>>(r =>
                r.StatusCode == StatusCodes.Status201Created
                && r.Value != null
                && r.Value.Equals(resultObject)
            );
    }

    [Theory]
    [MemberData(nameof(ToCreated_String_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data))]
    public void ToCreated_String_ShouldReturnCreatedHttpResult_WhenStateIsSuccess(
        string createdAtUri,
        TestResultObject resultObject
    )
    {
        ErrorOr<TestResultObject> errorOr = resultObject;

        var result = errorOr.ToCreated(createdAtUri);

        result
            .Should()
            .BeOfType<Created<TestResultObject>>()
            .And.Match<Created<TestResultObject>>(r =>
                r.StatusCode == StatusCodes.Status201Created
                && r.Value != null
                && r.Value.Equals(resultObject)
            );
    }

    [Theory]
    [MemberData(nameof(ToCreated_Uri_ShouldReturnProblemHttpResult_WhenStateIsError_Data))]
    public void ToCreated_Uri_ShouldReturnProblemHttpResult_WhenStateIsError(
        Uri createdAtUri,
        Error error,
        int expectedStatusCode
    )
    {
        ErrorOr<Success> errorOr = error;

        var result = errorOr.ToCreatedWithoutBody(createdAtUri);

        result
            .Should()
            .BeOfType<ProblemHttpResult>()
            .And.Match<ProblemHttpResult>(r =>
                r.StatusCode == expectedStatusCode
                && r.ContentType == MediaTypeNames.Application.ProblemJson
            );
    }

    [Theory]
    [MemberData(nameof(ToCreated_String_ShouldReturnProblemHttpResult_WhenStateIsError_Data))]
    public void ToCreated_String_ShouldReturnProblemHttpResult_WhenStateIsError(
        string createdAtUri,
        Error error,
        int expectedStatusCode
    )
    {
        ErrorOr<Success> errorOr = error;

        var result = errorOr.ToCreatedWithoutBody(createdAtUri);

        result
            .Should()
            .BeOfType<ProblemHttpResult>()
            .And.Match<ProblemHttpResult>(r =>
                r.StatusCode == expectedStatusCode
                && r.ContentType == MediaTypeNames.Application.ProblemJson
            );
    }

    [Fact]
    public void ToCreated_ShouldReturnValidationProblem_WhenStateIsError()
    {
        var testUri = new Uri("https://example.com/api/todos/1");
        ErrorOr<Success> errorOr = Error.Validation();

        var result = errorOr.ToCreated(testUri);

        result
            .Should()
            .BeOfType<ValidationProblem>()
            .And.Match<ValidationProblem>(r =>
                r.StatusCode == StatusCodes.Status400BadRequest
                && r.ContentType == MediaTypeNames.Application.ProblemJson
            );
    }

    [Fact]
    public void ToCreated_ShouldReturnCreatedHttpResult_AndMapToContract_WhenStateIsSuccess()
    {
        var testUri = new Uri("https://example.com/api/todos/1");
        var testResult = new TestResultObject("John", "Smith");
        var expectedContractResponse = new TestContractObject(
            $"{testResult.FirstName} {testResult.LastName}"
        );

        var mapperFunc = (TestResultObject resultObject) =>
            new TestContractObject($"{resultObject.FirstName} {resultObject.LastName}");

        ErrorOr<TestResultObject> errorOr = testResult;

        var result = errorOr.ToCreated(testUri, mapperFunc);

        result
            .Should()
            .BeOfType<Created<TestContractObject>>()
            .And.Match<Created<TestContractObject>>(r =>
                r.StatusCode == StatusCodes.Status201Created
                && r.Location == testUri.ToString()
                && r.Value == expectedContractResponse
            );
    }

    public static IEnumerable<object[]> ToCreated_Uri_ShouldReturnProblemHttpResult_WhenStateIsError_Data()
    {
        return new[]
        {
            new object[]
            {
                new Uri("https://example.com/api/todos/1"),
                Error.Forbidden(),
                StatusCodes.Status403Forbidden
            },

            [
                new Uri("https://example.com/api/todos/1"),
                Error.Unauthorized(),
                StatusCodes.Status401Unauthorized
            ],

            [
                new Uri("https://example.com/api/todos/1"),
                Error.Conflict(),
                StatusCodes.Status409Conflict
            ],

            [
                new Uri("https://example.com/api/todos/1"),
                Error.NotFound(),
                StatusCodes.Status404NotFound
            ],
        };
    }

    public static IEnumerable<object[]> ToCreated_String_ShouldReturnProblemHttpResult_WhenStateIsError_Data()
    {
        return new[]
        {
            new object[]
            {
                "https://example.com/api/todos/1",
                Error.Forbidden(),
                StatusCodes.Status403Forbidden
            },

            [
                "https://example.com/api/todos/1",
                Error.Unauthorized(),
                StatusCodes.Status401Unauthorized
            ],
            ["https://example.com/api/todos/1", Error.Conflict(), StatusCodes.Status409Conflict],
            ["https://example.com/api/todos/1", Error.NotFound(), StatusCodes.Status404NotFound],
        };
    }

    public static IEnumerable<object[]> ToCreated_Uri_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data()
    {
        return new[]
        {
            new object[]
            {
                new Uri("https://example.com/api/todos/1"),
                new TestResultObject("John", "Smith")
            }
        };
    }

    public static IEnumerable<object[]> ToCreated_String_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data()
    {
        return new[]
        {
            new object[]
            {
                "https://example.com/api/todos/1",
                new TestResultObject("John", "Smith")
            }
        };
    }

    public record TestResultObject(string FirstName, string LastName);

    public record TestContractObject(string Name);
}
