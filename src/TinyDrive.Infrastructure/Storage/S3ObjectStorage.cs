using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;

namespace TinyDrive.Infrastructure.Storage;

internal sealed class S3ObjectStorage(
    IAmazonS3 s3Client,
    IDateTimeProvider dateTimeProvider,
    IConfiguration configuration) : IObjectStorage
{
    private static readonly TimeSpan PresignedPostUrlTtl = TimeSpan.FromMinutes(5);

    private readonly string? _bucketName = configuration["ObjectStorage:BucketName"];

    public async Task<PresignedPostUrlData> GetPresignedPostUrlAsync(string key, long size, string contentType,
        CancellationToken cancellationToken = default)
    {
        DateTime expiresAtUtc = dateTimeProvider.UtcNow.Add(PresignedPostUrlTtl);

        var request = new CreatePresignedPostRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = expiresAtUtc,
            Conditions =
            [
                new ContentLengthRangeCondition(0, size + 10 * 1024),
            ]
        };

        CreatePresignedPostResponse? response = await s3Client.CreatePresignedPostAsync(request);

        var presignedPostUrlData = new PresignedPostUrlData
        {
            UploadUrl = response.Url,
            ExpiresAtUtc = expiresAtUtc,
            Fields = response.Fields
        };

        return presignedPostUrlData;
    }

    public async Task<ObjectMetaData> GetObjectMetaDataAsync(string key, CancellationToken cancellationToken = default)
    {
        var request = new GetObjectMetadataRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        GetObjectMetadataResponse response = await s3Client.GetObjectMetadataAsync(request, cancellationToken);

        return new ObjectMetaData
        {
            ContentType = response.Headers.ContentType,
            ContentLength = response.Headers.ContentLength,
            LastModifiedAtUtc = response.LastModified
        };
    }
}
