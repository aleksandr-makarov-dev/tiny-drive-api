using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;

namespace TinyDrive.Infrastructure.Storage;

internal sealed class S3ObjectStorage(IAmazonS3 s3Client, IConfiguration configuration) : IObjectStorage
{
    private readonly string? _bucketName = configuration["ObjectStorage:BucketName"];

    public async Task<PresignedPostData> CreatePresignedPostAsync(string key, long size,
        string contentType, DateTime expiresAtUtc)
    {
        var request = new CreatePresignedPostRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = expiresAtUtc,
            Conditions =
            [
                new ContentLengthRangeCondition(1, size + 10_240),
                new ExactMatchCondition("Content-Type", contentType)
            ]
        };

        CreatePresignedPostResponse? result = await s3Client.CreatePresignedPostAsync(request);

        return new PresignedPostData
        {
            Url = result.Url,
            Fields = result.Fields,
            ExpiresAtUtc = expiresAtUtc
        };
    }

    public async Task<ObjectAttributesData> GetObjectStatsAsync(string key)
    {
        var request = new GetObjectAttributesRequest
        {
            BucketName = _bucketName,
            Key = key,
            ObjectAttributes =
            [
                ObjectAttributes.ObjectSize,
                ObjectAttributes.ETag
            ]
        };

        GetObjectAttributesResponse? result = await s3Client.GetObjectAttributesAsync(request);

        return new ObjectAttributesData
        {
            ETag = result.ETag,
            ObjectSize = result.ObjectSize
        };
    }
}
