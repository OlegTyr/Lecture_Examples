using LogHub.Domain;
using LogHub.Filtering;
using LogHub.Formatting;
using LogHub.Parsing;
using LogHub.Processing;
using LogHub.Reading;
using LogHub.Routing;
using LogHub.Writing;
using Xunit;

namespace LogHub.Tests;

public class LogProcessorTests
{
    private static LogProcessor Build(
        IEnumerable<string> lines,
        LogLevel min = LogLevel.Debug,
        ILogSink? sink = null)
    {
        var actualSink = sink ?? new InMemoryLogSink();
        var resolver = new DictionarySinkResolver(new (string, ILogSink)[]
        {
            ("m", actualSink),
        });
        return new LogProcessor(
            new InMemoryLogReader(lines),
            new SimpleLogParser(),
            new ILogFilter[] { new SeverityFilter(min) },
            new TextLogFormatter(),
            resolver,
            "m");
    }

    [Fact]
    public void ParsesValidLine()
    {
        var result = Build(new[] { "2026-01-15 10:00:00|Info|ok" }).Run();
        Assert.Equal(1, result.Written);
        Assert.Equal(0, result.Skipped);
    }

    [Fact]
    public void SkipsBadLine()
    {
        var result = Build(new[] { "garbage" }).Run();
        Assert.Equal(0, result.Written);
        Assert.Equal(1, result.Skipped);
    }

    [Fact]
    public void FiltersBySeverity()
    {
        var result = Build(new[]
        {
            "2026-01-15 10:00:00|Info|a",
            "2026-01-15 10:00:00|Debug|b",
        }, LogLevel.Info).Run();

        Assert.Equal(1, result.Written);
        Assert.Equal(1, result.Skipped);
    }

    [Fact]
    public void EmptyInput_ReturnsZeros()
    {
        var result = Build(Array.Empty<string>()).Run();
        Assert.Equal(0, result.Written);
        Assert.Equal(0, result.Skipped);
    }

    [Fact]
    public void WritesFormattedLine()
    {
        var sink = new InMemoryLogSink();
        Build(new[] { "2026-01-15 10:00:00|Info|hi" }, LogLevel.Debug, sink).Run();

        Assert.Single(sink.Lines);
        Assert.Contains("Info: hi", sink.Lines[0]);
    }

    [Fact]
    public void UnknownSink_Throws()
    {
        var resolver = new DictionarySinkResolver(Array.Empty<(string, ILogSink)>());
        var processor = new LogProcessor(
            new InMemoryLogReader(new[] { "2026-01-15 10:00:00|Info|x" }),
            new SimpleLogParser(),
            Array.Empty<ILogFilter>(),
            new TextLogFormatter(),
            resolver,
            "missing");

        Assert.Throws<KeyNotFoundException>(() => processor.Run());
    }
}