namespace LogHub.Writing;

public sealed class TelegramSink(string botToken) : ILogSink
{
    public void Write(string line)
    {
        _ = botToken;
    }
}