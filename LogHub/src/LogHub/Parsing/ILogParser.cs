using LogHub.Domain;

namespace LogHub.Parsing;

public interface ILogParser
{
    LogEntry? TryParse(string line);
}

public sealed class SimpleLogParser : ILogParser
{
    public LogEntry? TryParse(string line)
    {
        try { return LogEntry.Parse(line); }
        catch { return null; }
    }
}