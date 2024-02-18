using Chatty.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Interfaces
{
    public interface IChat : IGrainObserver
    {
        Task ReceiveMessage(Message message);
    }
}
