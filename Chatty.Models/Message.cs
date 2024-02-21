using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public String? Content {  get; set; }

        public DateTime CreatedAt { get; set; }

        public required User Creator { get; set; }

    }
}
