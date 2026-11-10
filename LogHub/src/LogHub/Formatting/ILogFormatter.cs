using LogHub.Domain;

namespace LogHub.Formatting;

public interface ILogFormatter
{
    string Format(LogEntry entry);
}

public sealed class TextLogFormatter : ILogFormatter
{
    public string Format(LogEntry entry) =>
        $"[{entry.Timestamp:u}] {entry.Level}: {entry.Message}";
}