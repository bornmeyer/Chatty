
using Orleans.Services;

namespace Chatty.Models
{
    public interface IRoomRegistrarService : IGrainService
    {
        Task<IEnumerable<Guid>> GetRoomIds();
        Task Register(Guid roomId);
        Task Unregister(Guid roomId);
    }
}