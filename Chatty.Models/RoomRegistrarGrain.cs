using Chatty.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class RoomRegistrarGrain : Grain, IRoomRegistrarGrain
    {
        // Fields

        private readonly IRoomRegistrarServiceClient roomRegistrarServiceClient = default!;
        private readonly ObserverManager<IRoomObserver> roomSubsManager;


        // Constructors

        public RoomRegistrarGrain(IRoomRegistrarServiceClient roomRegistrarServiceClient, ILogger<RoomRegistrarGrain> logger)
        {
            this.roomRegistrarServiceClient = roomRegistrarServiceClient;
            this.roomSubsManager = new ObserverManager<IRoomObserver>(TimeSpan.FromMinutes(5), logger);
        }


        // Methods

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await roomSubsManager.Notify(s => s.RoomSubscriptionsChanged());
            await base.OnActivateAsync(cancellationToken);
        }

        public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            await roomSubsManager.Notify(s => s.RoomSubscriptionsChanged());
            await base.OnDeactivateAsync(reason, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> GetRooms()
        {
            return await this.roomRegistrarServiceClient.GetRoomIds();
        }

        public async Task Subscribe(IRoomObserver observer) => this.roomSubsManager.Subscribe(observer, observer);

        public async Task Unsubscribe(IRoomObserver observer) => this.roomSubsManager?.Unsubscribe(observer);

        public async Task NotifyUpdate()
        {
            await roomSubsManager.Notify(s => s.RoomSubscriptionsChanged());
        }
    }
}
