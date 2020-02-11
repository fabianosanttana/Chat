using Chat.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Context
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options)
          : base(options)
        {}
        public DbSet<User> Users { get; set; }
        //public DbSet<Post> Posts { get; set; }
    }
}