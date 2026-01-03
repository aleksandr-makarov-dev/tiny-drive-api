using TinyDrive.Application.Abstract.Time;

namespace TinyDrive.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
