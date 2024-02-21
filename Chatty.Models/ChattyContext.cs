using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class ChattyContext : DbContext
    {
        // Fields

        private readonly String _dbPath = default!;

        // Properties

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<PersistedRoomState> RoomStates { get; set; }

        // Constructors

        public ChattyContext()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dbPath = Path.Join(folder, "OrleansTest", "chatty.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        
    }
}
