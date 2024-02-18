using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Interfaces.DTOs
{
    [GenerateSerializer]
    public class Message
    {
        [Id(0)]
        public String Content { get; set; }

        [Id(1)]
        public DateTime Timestamp { get; set; }
    }
}
