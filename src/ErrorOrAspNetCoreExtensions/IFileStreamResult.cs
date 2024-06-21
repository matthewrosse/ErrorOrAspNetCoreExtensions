namespace ErrorOrAspNetCoreExtensions;

public interface IFileStreamResult
{
    Stream FileContent { get; }
    string? ContentType { get; }
    string? DownloadFileName { get; }
    DateTimeOffset? LastModified { get; }
}
