namespace TinyDrive.Application.Abstract.Time;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
