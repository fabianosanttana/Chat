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
        public DbSet<ChatMessage> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(m => m.key);
            builder.Entity<ChatMessage>().HasKey(m => m.key);
            builder.Entity<ChatMessage>().HasOne(m => m.from).WithMany().HasForeignKey(u => u.fromId);
            builder.Entity<ChatMessage>().HasOne(m => m.to).WithMany().HasForeignKey(u => u.toId);

            base.OnModelCreating(builder);
        }
    }
}