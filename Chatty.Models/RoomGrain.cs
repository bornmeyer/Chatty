using Chatty.Interfaces;
using Chatty.Interfaces.DTOs;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Utilities;

namespace Chatty.Models
{
    public class RoomGrain : Grain, IRoomGrain
    {
        // Fields

        private readonly IPersistentState<RoomState> state;
        private readonly ObserverManager<IChat> subsManager;

        // Constructors

        public RoomGrain([PersistentState("RoomState", "RoomStateStorage")] IPersistentState<RoomState> state, ILogger<RoomGrain> logger)
        {
            this.state = state;
            subsManager = new ObserverManager<IChat>(TimeSpan.FromMinutes(5), logger);

        }

        // Methods

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await state.ReadStateAsync();
            await base.OnActivateAsync(cancellationToken);
        }

        public async Task AppendMessage(string message)
        {
            var newMessage = new Message { Content = message, Timestamp = DateTime.Now };

            state.State.ChatMessages.Add(newMessage);
            await subsManager.Notify(s => s.ReceiveMessage(newMessage));
            await state.WriteStateAsync();            
        }

        public async Task<List<Message>> GetMessages()
        {
            return state.State.ChatMessages;
        }

        public async Task Subscribe(IChat observer)
        {
            this.subsManager.Subscribe(observer, observer);
        }

        public async Task Unsubscribe(IChat observer)
        {
            this.subsManager.Unsubscribe(observer);
        }
    }
}
