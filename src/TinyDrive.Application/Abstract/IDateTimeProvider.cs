namespace TinyDrive.Application.Abstract;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
