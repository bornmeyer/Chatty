
using Chatty.Interfaces.DTOs;

namespace Chatty.Web.Models
{
    public interface IMessageService
    {
        event Action<Message>? OnMessage;

        Task BroadcastMessage(Guid roomId, String message);
        Task SendMessage(Guid receiver);

        Task<List<Message>> GetMessages(Guid roomId);

        Task Subscribe(Guid roomId);
    }
}