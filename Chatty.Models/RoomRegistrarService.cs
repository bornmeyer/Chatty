using Chatty.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public sealed class RoomRegistrarService : GrainService, IRoomRegistrarService
    {
        // Fields

        private readonly IGrainFactory grainFactory = default!;

        private readonly List<Guid> roomIds = [];

        // Constructors

        public RoomRegistrarService(IServiceProvider services,
            GrainId id,
            Silo silo,
            ILoggerFactory loggerFactory,
            IGrainFactory grainFactory)
        : base(id, silo, loggerFactory)
        {
            this.grainFactory = grainFactory;
            this.roomIds = [];
        }

        public async Task<IEnumerable<Guid>> GetRoomIds() => await Task.FromResult<IEnumerable<Guid>>(roomIds);

        public async Task Register(Guid roomId) => await OnStateChange(roomId, this.roomIds.Add);

        public async Task Unregister(Guid roomId) => await OnStateChange(roomId, x => this.roomIds.Remove(x));

        private async Task OnStateChange(Guid roomId, Action<Guid> action)
        {
            action?.Invoke(roomId);
            IRoomRegistrarGrain roomRegistrarGrain = this.grainFactory.GetGrain<IRoomRegistrarGrain>(Guid.Empty);
            await roomRegistrarGrain.NotifyUpdate();
        }
    }
}
