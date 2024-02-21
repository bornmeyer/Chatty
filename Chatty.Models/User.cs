using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public String? Email { get; set; }

        public String? PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual required List<Message> Messages { get; set; }
    }
}
