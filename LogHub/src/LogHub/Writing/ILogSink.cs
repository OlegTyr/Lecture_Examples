namespace LogHub.Writing;

public interface ILogSink
{
    void Write(string formattedLine);
}

public sealed class ConsoleLogSink : ILogSink
{
    public void Write(string line) => Console.WriteLine(line);
}

public sealed class InMemoryLogSink : ILogSink
{
    private readonly List<string> _buffer = new();
    public IReadOnlyList<string> Lines => _buffer;
    public void Write(string line) => _buffer.Add(line);
}