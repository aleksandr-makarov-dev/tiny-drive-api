namespace TinyDrive.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
