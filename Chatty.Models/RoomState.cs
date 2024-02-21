using Chatty.Interfaces.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class RoomState
    {
        public List<Chatty.Interfaces.DTOs.Message> ChatMessages { get; set; } = [];
    }
}
