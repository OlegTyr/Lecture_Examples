using LogHub.Domain;

namespace LogHub.Filtering;

public interface ILogFilter
{
    bool ShouldPass(LogEntry entry);
}

public sealed class SeverityFilter(LogLevel minimum) : ILogFilter
{
    public bool ShouldPass(LogEntry entry) => entry.Level >= minimum;
}