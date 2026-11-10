namespace LogHub.Reading;

public interface ILogReader
{
    IEnumerable<string> ReadLines();
}

public sealed class FileLogReader(string path) : ILogReader
{
    public IEnumerable<string> ReadLines() => File.ReadAllLines(path);
}

public sealed class InMemoryLogReader(IEnumerable<string> lines) : ILogReader
{
    public IEnumerable<string> ReadLines() => lines;
}