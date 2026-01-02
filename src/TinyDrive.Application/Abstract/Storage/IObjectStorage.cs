using TinyDrive.Application.Abstract.Storage.Models;

namespace TinyDrive.Application.Abstract.Storage;

public interface IObjectStorage
{
    Task<PresignedPostData> CreatePresignedPostAsync(string key, long fileSize, string contentType);
}
