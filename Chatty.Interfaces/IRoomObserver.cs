using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Interfaces
{
    public interface IRoomObserver : IGrainObserver
    {
        Task RoomSubscriptionsChanged();
    }
}
