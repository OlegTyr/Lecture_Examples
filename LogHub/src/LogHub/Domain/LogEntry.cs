using System.Globalization;

namespace LogHub.Domain;

public enum LogLevel { Debug, Info, Warning, Error, Critical }

public sealed record LogEntry(
    DateTime Timestamp,
    LogLevel Level,
    string Message,
    string? Source = null)
{
    public static LogEntry Parse(string line)
    {
        var parts = line.Split('|', 3);
        if (parts.Length < 3)
            throw new FormatException($"Bad log line: {line}");

        var ts = DateTime.Parse(parts[0], CultureInfo.InvariantCulture);
        var lvl = Enum.Parse<LogLevel>(parts[1], ignoreCase: true);
        return new LogEntry(ts, lvl, parts[2]);
    }
}