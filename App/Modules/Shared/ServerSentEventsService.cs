using System.Collections.Concurrent;
using System.Threading.Channels;

namespace lanstreamer_api.services;

public class ServerSentEventsService<T>
{
    private readonly ConcurrentDictionary<string, Channel<T>> _channels = new();

    public ChannelReader<T> Subscribe(string key)
    {
        var channel = Channel.CreateUnbounded<T>();
        _channels[key] = channel;
        return channel.Reader;
    }

    public async Task Send(string key, T data)
    {
        if (_channels.TryGetValue(key, out var channel))
        {
            await channel.Writer.WriteAsync(data);
        }
    }
}