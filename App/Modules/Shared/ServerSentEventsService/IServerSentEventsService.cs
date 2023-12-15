using System.Threading.Channels;

namespace lanstreamer_api.services;

public interface IServerSentEventsService<T>
{
    ChannelReader<T> Subscribe(string key);
    void Unsubscribe(string key);
    Task Send(string key, T data);
}