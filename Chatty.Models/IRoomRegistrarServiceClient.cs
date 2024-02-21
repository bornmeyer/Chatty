using Orleans.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public interface IRoomRegistrarServiceClient : 
        IGrainServiceClient<IRoomRegistrarService>, IRoomRegistrarService
    {

    }
}
