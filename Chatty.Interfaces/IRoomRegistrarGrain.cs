using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Interfaces
{
    public interface IRoomRegistrarGrain : IGrainWithGuidKey
    {
        Task<IEnumerable<Guid>> GetRooms();

        Task Subscribe(IRoomObserver observer);

        Task Unsubscribe(IRoomObserver observer);

        Task NotifyUpdate();
    }
}
