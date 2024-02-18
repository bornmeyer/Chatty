using Chatty.Interfaces.DTOs;
using Orleans;

namespace Chatty.Interfaces
{
    public interface IRoomGrain : IGrainWithGuidKey
    {
        Task<List<Message>> GetMessages();

        Task AppendMessage(String message);

        Task Subscribe(IChat observer);

        Task Unsubscribe(IChat observer);
    }
}
