using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class PersistedRoomState
    {
        // Properties

        public Guid? Id { get; set; }

        public Guid? GrainId {  get; set; }

        public String? State { get; set; }
    }
}
