using LogHub.Domain;
using LogHub.Filtering;
using LogHub.Formatting;
using LogHub.Parsing;
using LogHub.Processing;
using LogHub.Reading;
using LogHub.Routing;
using LogHub.Writing;

var memorySink = new InMemoryLogSink();
var consoleSink = new ConsoleLogSink();

var resolver = new DictionarySinkResolver(new (string, ILogSink)[]
{
    ("console", consoleSink),
    ("memory", memorySink),
});

var inputPath = args.Length > 0 ? args[0] : "app.log";
var sinkName = args.Length > 1 ? args[1] : "console";

var processor = new LogProcessor(
    new FileLogReader(inputPath),
    new SimpleLogParser(),
    new ILogFilter[] { new SeverityFilter(LogLevel.Info) },
    new TextLogFormatter(),
    resolver,
    sinkName);

var result = processor.Run();
Console.WriteLine($"written={result.Written}, skipped={result.Skipped}");