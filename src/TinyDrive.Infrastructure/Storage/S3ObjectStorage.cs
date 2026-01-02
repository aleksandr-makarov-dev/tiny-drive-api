using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;

namespace TinyDrive.Infrastructure.Storage;

internal sealed class S3ObjectStorage(IAmazonS3 s3Client, IConfiguration configuration) : IObjectStorage
{
    public async Task<PresignedPostData> CreatePresignedPostAsync(string key, long size,
        string contentType)
    {
        DateTime expiresOnUtc = DateTime.UtcNow.AddMinutes(15);

        var request = new CreatePresignedPostRequest
        {
            BucketName = configuration["ObjectStorage:BucketName"],
            Key = key,
            Expires = expiresOnUtc,
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
            ExpiresOnUtc = expiresOnUtc
        };
    }
}
