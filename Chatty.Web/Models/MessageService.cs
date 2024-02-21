using Chatty.Interfaces;
using Chatty.Interfaces.DTOs;
using Chatty.Web.Components.Pages;
//using Orleans.Stream;

namespace Chatty.Web.Models
{
    public class MessageService : IMessageService, IChat
    {
        // Events

        public event Action<Message>? OnMessage;

        // Fields

        private readonly IClusterClient clusterClient;
        private IChat? observer = default;

        // Constructors

        public MessageService(IClusterClient clusterClient)
        {
            this.clusterClient = clusterClient;
        }

        // Methods

        public async Task SendMessage(Guid receiver) => throw new NotImplementedException();

        public async Task BroadcastMessage(Guid roomId, String message)
        {
            IRoomGrain grain = clusterClient.GetGrain<IRoomGrain>(roomId);            
            await grain.AppendMessage(message);
        }

        public async Task<List<Message>> GetMessages(Guid roomId)
        {
            IRoomGrain grain = clusterClient.GetGrain<IRoomGrain>(roomId);
            return await grain.GetMessages();
        }

        public async Task ReceiveMessage(Message message)
        {
            OnMessage?.Invoke(message);
        }

        public async Task Subscribe(Guid roomId)
        {
            IRoomGrain grain = clusterClient.GetGrain<IRoomGrain>(roomId);
            observer = clusterClient.CreateObjectReference<IChat>(this);
            await grain.Subscribe(observer);
        }

    }
}
