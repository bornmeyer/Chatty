using Orleans.Runtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class RoomRegistrarServiceClient : GrainServiceClient<IRoomRegistrarService>, IRoomRegistrarServiceClient
    {
        // Fields

        private IRoomRegistrarService GrainService => GetGrainService(CurrentGrainReference.GrainId);

        // Constructors

        public RoomRegistrarServiceClient(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
                
        }


        // Methods

        public async Task<IEnumerable<Guid>> GetRoomIds()
        {
            return await GrainService.GetRoomIds();
        }

        public async Task Register(Guid roomId)
        {
            await GrainService.Register(roomId);
        }

        public async Task Unregister(Guid roomId)
        {
            await GrainService.Unregister(roomId);
        }
    }
}
