using System.Net.Mime;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using EntityTagHeaderValue = Microsoft.Net.Http.Headers.EntityTagHeaderValue;

namespace ErrorOrAspNetCoreExtensions.Tests.Unit;

public class ToFileStreamTests
{
    [Fact]
    public void ToFileStream_ShouldReturnFileStreamHttpResult_WhenResultStateIsSuccess()
    {
        // Arrange
        using var stream = new MemoryStream(TestPayload);
        var contentType = MediaTypeNames.Application.Octet;
        var downloadFileName = "downloadFileName";
        var lastModified = new DateTimeOffset(new DateTime(1970, 1, 1));
        var enableRangeProcessing = false;
        var entityTag = EntityTagHeaderValue.Any;

        ErrorOr<IFileStreamResult> errorOr = new TestFileStreamResult(
            stream,
            contentType,
            downloadFileName,
            lastModified
        );

        // Act

        var result = errorOr.ToFileStream(enableRangeProcessing, entityTag);

        result
            .Should()
            .BeOfType<FileStreamHttpResult>()
            .And.Match<FileStreamHttpResult>(r =>
                r.ContentType == contentType
                && r.LastModified == lastModified
                && r.EnableRangeProcessing == enableRangeProcessing
                && r.FileDownloadName == downloadFileName
                && r.EntityTag == entityTag
            );

        result
            .Should()
            .BeOfType<FileStreamHttpResult>()
            .Which.FileStream.Should()
            .BeSameAs(stream);
    }

    private static byte[] TestPayload => [0x3F, 0x7A, 0xA1, 0xD4, 0x9C, 0x4B, 0xE8, 0x13];

    private record TestFileStreamResult(
        Stream FileContent,
        string? ContentType = null,
        string? DownloadFileName = null,
        DateTimeOffset? LastModified = null
    ) : IFileStreamResult;
}
