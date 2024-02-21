
namespace Chatty.Web.Models
{
    public interface IRoomService
    {
        event Action OnRoomsChanged;

        Task<IEnumerable<Guid>> GetRoomIds();

        Task Subscribe();

        Task Unsubscribe();
    }
}