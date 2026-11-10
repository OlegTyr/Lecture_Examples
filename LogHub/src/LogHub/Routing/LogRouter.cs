using LogHub.Writing;

namespace LogHub.Routing;

public interface ILogSinkResolver
{
    ILogSink Resolve(string sinkName);
}

public sealed class DictionarySinkResolver : ILogSinkResolver
{
    private readonly Dictionary<string, ILogSink> _map;

    public DictionarySinkResolver(IEnumerable<(string name, ILogSink sink)> sinks)
    {
        _map = new Dictionary<string, ILogSink>();
        foreach (var (name, sink) in sinks)
            _map[name] = sink;
    }

    public ILogSink Resolve(string sinkName) =>
        _map.TryGetValue(sinkName, out var sink)
            ? sink
            : throw new KeyNotFoundException($"Sink '{sinkName}' not registered");
}