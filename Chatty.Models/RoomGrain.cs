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
        private readonly IRoomRegistrarServiceClient roomRegistrarServiceClient;
        private readonly IGrainFactory grainFactory;
        private readonly ObserverManager<IChat> messageSubsManager;

        // Properties

        public Guid Id
        {
            get
            {
                return this.GetPrimaryKey();
            }
        }

        // Constructors

        public RoomGrain([PersistentState("RoomState", "RoomStateStorage")] IPersistentState<RoomState> state, 
            IRoomRegistrarServiceClient roomRegistrarServiceClient,
            ChattyContext context,
            IGrainFactory grainFactory,
            ILogger<RoomGrain> logger)
        {
            this.state = state;
            this.roomRegistrarServiceClient = roomRegistrarServiceClient;
            this.grainFactory = grainFactory;
            this.messageSubsManager = new ObserverManager<IChat>(TimeSpan.FromMinutes(5), logger);
        }

        // Methods

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await state.ReadStateAsync();
            await this.roomRegistrarServiceClient.Register(Id);
            await base.OnActivateAsync(cancellationToken);
        }

        public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            await this.roomRegistrarServiceClient.Unregister(Id);
            await base.OnDeactivateAsync(reason, cancellationToken);
        }

        public async Task AppendMessage(string message)
        {
            var newMessage = new Chatty.Interfaces.DTOs.Message { Content = message, Timestamp = DateTime.Now };

            state.State.ChatMessages.Add(newMessage);
            await messageSubsManager.Notify(s => s.ReceiveMessage(newMessage));
            await state.WriteStateAsync();
        }

        public async Task<List<Chatty.Interfaces.DTOs.Message>> GetMessages()
        {
            return state.State.ChatMessages;
        }

        public async Task Subscribe(IChat observer)
        {
            this.messageSubsManager.Subscribe(observer, observer);
        }        

        public async Task Unsubscribe(IChat observer)
        {
            this.messageSubsManager.Unsubscribe(observer);
        }
    }
}
