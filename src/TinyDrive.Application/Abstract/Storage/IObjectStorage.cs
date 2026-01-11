using TinyDrive.Application.Abstract.Storage.Models;

namespace TinyDrive.Application.Abstract.Storage;

public interface IObjectStorage
{
    Task<PresignedPostUrlData> GetPresignedPostUrlAsync(string key, long size, string contentType,
        CancellationToken cancellationToken = default);
}
