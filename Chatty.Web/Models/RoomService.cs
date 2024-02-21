using Chatty.Interfaces;

namespace Chatty.Web.Models
{
    public class RoomService : IRoomService, IRoomObserver
    {
        // Events

        public event Action? OnRoomsChanged;

        // Fields

        private readonly IClusterClient clusterClient = default!;
        private readonly IRoomObserver observer = default!;

        // Constructors

        public RoomService(IClusterClient clusterClient)
        {
            this.clusterClient = clusterClient;
            this.observer = clusterClient.CreateObjectReference<IRoomObserver>(this);

        }

        // Methods

        public async Task Subscribe()
        {
            IRoomRegistrarGrain roomRegistrarGrain =
                this.clusterClient.GetGrain<IRoomRegistrarGrain>(Guid.Empty);
            await roomRegistrarGrain.Subscribe(this.observer);
        }

        public async Task Unsubscribe()
        {
            IRoomRegistrarGrain roomRegistrarGrain =
                this.clusterClient.GetGrain<IRoomRegistrarGrain>(Guid.Empty);
            await roomRegistrarGrain.Unsubscribe(this.observer);
        }

        public async Task<IEnumerable<Guid>> GetRoomIds()
        {
            IRoomRegistrarGrain grain = this.clusterClient.GetGrain<IRoomRegistrarGrain>(Guid.Empty);
            return await grain.GetRooms();
        }

        public async Task RoomSubscriptionsChanged()
        {
            OnRoomsChanged?.Invoke();
        }
    }
}
