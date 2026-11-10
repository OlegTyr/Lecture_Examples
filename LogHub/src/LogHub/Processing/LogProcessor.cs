using LogHub.Filtering;
using LogHub.Formatting;
using LogHub.Parsing;
using LogHub.Reading;
using LogHub.Routing;

namespace LogHub.Processing;

public sealed class LogProcessor(
    ILogReader reader,
    ILogParser parser,
    IEnumerable<ILogFilter> filters,
    ILogFormatter formatter,
    ILogSinkResolver resolver,
    string sinkName)
{
    public (int Written, int Skipped) Run()
    {
        int written = 0, skipped = 0;
        var sink = resolver.Resolve(sinkName);

        foreach (var line in reader.ReadLines())
        {
            var entry = parser.TryParse(line);
            if (entry is null) { skipped++; continue; }

            var passes = true;
            foreach (var filter in filters)
                if (!filter.ShouldPass(entry)) { passes = false; break; }
            if (!passes) { skipped++; continue; }

            sink.Write(formatter.Format(entry));
            written++;
        }

        return (written, skipped);
    }
}