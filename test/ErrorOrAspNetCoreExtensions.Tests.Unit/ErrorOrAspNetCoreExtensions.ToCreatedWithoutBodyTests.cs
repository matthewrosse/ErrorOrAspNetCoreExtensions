using System.Net.Mime;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Created = Microsoft.AspNetCore.Http.HttpResults.Created;

namespace ErrorOrAspNetCoreExtensions.Tests.Unit;

public class ToCreatedWithoutBodyTests
{
    [Fact]
    public void ToCreatedWithoutBody_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_AndNoUriIsProvided()
    {
        ErrorOr<Success> errorOr = new Success();

        var result = errorOr.ToCreatedWithoutBody();

        result.Should().BeOfType<Created>().Which.Location.Should().BeNull();
    }

    [Theory]
    [MemberData(
        nameof(ToCreatedWithoutBody_Uri_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data)
    )]
    public void ToCreatedWithoutBody_Uri_ShouldReturnCreatedHttpResult_WhenStateIsSuccess(
        Uri createdAtUri
    )
    {
        ErrorOr<TestObject> errorOr = new TestObject("value");

        var result = errorOr.ToCreatedWithoutBody(createdAtUri);

        result
            .Should()
            .BeOfType<Created>()
            .And.Match<Created>(r =>
                r.StatusCode == StatusCodes.Status201Created
                && r.Location == createdAtUri.ToString()
            );
    }

    [Theory]
    [MemberData(
        nameof(ToCreatedWithoutBody_String_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data)
    )]
    public void ToCreatedWithoutBody_String_ShouldReturnCreatedHttpResult_WhenStateIsSuccess(
        string createdAtUri
    )
    {
        ErrorOr<Success> errorOr = new Success();

        var result = errorOr.ToCreatedWithoutBody(createdAtUri);

        result
            .Should()
            .BeOfType<Created>()
            .And.Match<Created>(r =>
                r.StatusCode == StatusCodes.Status201Created
                && r.Location == createdAtUri.ToString()
            );
    }

    [Theory]
    [MemberData(
        nameof(ToCreatedWithoutBody_Uri_ShouldReturnProblemHttpResult_WhenStateIsError_Data)
    )]
    public void ToCreatedWithoutBody_Uri_ShouldReturnProblemHttpResult_WhenStateIsError(
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
    [MemberData(
        nameof(ToCreatedWithoutBody_String_ShouldReturnProblemHttpResult_WhenStateIsError_Data)
    )]
    public void ToCreatedWithoutBody_String_ShouldReturnProblemHttpResult_WhenStateError(
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

    public static IEnumerable<object[]> ToCreatedWithoutBody_Uri_ShouldReturnProblemHttpResult_WhenStateIsError_Data()
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

    public static IEnumerable<object[]> ToCreatedWithoutBody_String_ShouldReturnProblemHttpResult_WhenStateIsError_Data()
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

    public static IEnumerable<object[]> ToCreatedWithoutBody_Uri_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data()
    {
        return new[]
        {
            new object[] { new Uri("https://example.com/api/todos/1") },
            [new Uri("/api/todos/420")]
        };
    }

    public static IEnumerable<object[]> ToCreatedWithoutBody_String_ShouldReturnCreatedHttpResult_WhenStateIsSuccess_Data()
    {
        return new[] { new object[] { "https://example.com/api/todos/1" }, ["/api/todos/420"] };
    }

    private record TestObject(string SomeValue);
}
